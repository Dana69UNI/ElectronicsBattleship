using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// For Serial
using System.IO.Ports;
// For compare array
using System.Linq;

public class Arduino : MonoBehaviour
{

	// Serial
	public string portName;
	public int baudRate = 9600;
	public string str;
	
	private int ValorArduinoSensor;
	private int ValorArduinoSensorAnterior=0;
	private int ValorDistanciaPlayer1;
    private int ValorDistanciaPlayer2;

    public GameObject PulsaParaIniciar;
    public GameObject P1ColocaBarco;
    public GameObject P2ColocaBarco;
    public GameObject P1Ataque;
    public GameObject P2Ataque;
    public GameObject Agua;
    public GameObject P1HASGANAU;
    public GameObject P2HASGANAU;

    public int sensor;
	private int Fase=0; //Fase 0 = juego no comenzado Fase 1 = Placement 1, Fase 2 Placement 2 ...
    SerialPort arduinoSerial = new SerialPort("COM3", 9600);

    void Start()
	{
        PulsaParaIniciar.SetActive(true);
        // Open Serial port

        //// Set buffersize so read from Serial would be normal
        //arduinoSerial.ReadTimeout = 1;
        //arduinoSerial.ReadBufferSize = 8192;
        //arduinoSerial.WriteBufferSize = 128;
        //arduinoSerial.ReadBufferSize = 4096;
        //arduinoSerial.Parity = Parity.None;
        //arduinoSerial.StopBits = StopBits.One;
        //arduinoSerial.DtrEnable = true;
        //arduinoSerial.RtsEnable = true; 
        arduinoSerial.ReadTimeout = 100;
        arduinoSerial.Open();
	}

	void Update()
	{
		ReadFromArduino();

		//str = arduinoSerial.ReadLine();
		//Debug.Log(str);
		

	}

	public void ReadFromArduino()
	{
		
		try
		{
			str = arduinoSerial.ReadLine();
				Debug.Log(str);
			
			if (Int32.TryParse(str, out ValorArduinoSensor))
			{
				//sensor = ValorArduinoSensor;
				if(ValorArduinoSensor !=0)
                {
					CheckArduinoNumber(str);
					GameHandler(str);
					//Debug.Log(ValorArduinoSensor);
				}
				
			}
		}
		catch (TimeoutException e)
		{
		}
	}

    private void CheckArduinoNumber(string str)
    {
		if(str == "999")
		{
            PulsaParaIniciar.SetActive(false);
            P1ColocaBarco.SetActive(true) ;
            Fase = 0;
            Debug.Log("Comienza placement player 1");
		}
        if (str == "010101")
		{
			Fase= 1;
            Debug.Log("Placement player 1");
            //despues Placement 1 / antes ataque 1
        }
        if (str == "666")
        {
            P1ColocaBarco.SetActive(false);
            P2ColocaBarco.SetActive(true);
            Fase = 2;
            Debug.Log("Comienza placement player 2");
        }
        if (str == "020202")
        {
			Fase = 3;
            //despues Placement 2 / antes ataque 1
        }
        if (str == "151515")
        {
            P2ColocaBarco.SetActive(false);
            P1Ataque.SetActive(true);
            Fase = 4;
            Debug.Log("Comienza Ataque 1");
            //antes ataque 1
        }
        if (str == "121212")
        {
            Fase = 5;
            //despues ataque 1 / antes ataque 1

        }
        if (str == "252525")
        {
            P1Ataque.SetActive(false);
            P2Ataque.SetActive(true);
            Fase = 6;
            //antes ataque 2
            Debug.Log("Comienza Ataque 2");
        }
        if (str == "212121")
        {
            Fase = 7;
            //despues ataque 2 / antes ataque 1
        }
        if (str == "555")
        {
            if (P1Ataque.activeSelf == true)
            {
                P1Ataque.SetActive(false);
                P1HASGANAU.SetActive(true);
            }
            if (P2Ataque.activeSelf == true)
            {
                P2Ataque.SetActive(false);
                P2HASGANAU.SetActive(true);
            }
           
            
            StartCoroutine(HasGanaoTurnOff());
            //despues Win
            Debug.Log("HAS GANAU TOMAYA");
        }
        if (str == "444")
        {
            P2Ataque.SetActive(false);
            Agua.SetActive(true);
            StartCoroutine(WaterTurnOff());
            //despues Water
            Debug.Log("AGUA:P");
        }
    }

    IEnumerator HasGanaoTurnOff()
    {
        yield return new WaitForSeconds(9.5f);
        P1HASGANAU.SetActive(false);
        P2HASGANAU.SetActive(false);
        PulsaParaIniciar.SetActive(true);
    }
    IEnumerator WaterTurnOff()
    {
        yield return new WaitForSeconds(4.5f);
        Agua.SetActive(false);
        PulsaParaIniciar.SetActive(true);
    }

    private void GameHandler(string str)
	{
		ValorArduinoSensorAnterior = ValorArduinoSensor;
		Int32.TryParse(str, out ValorArduinoSensor);
		if(Fase ==1)
		{
			Debug.Log(str);
		}
		if(Fase == 2)
		{
			if(ValorArduinoSensorAnterior != 010101)
			{
				ValorDistanciaPlayer1 = ValorArduinoSensorAnterior;
				Debug.Log(ValorDistanciaPlayer1);
			}
		}

    }
}
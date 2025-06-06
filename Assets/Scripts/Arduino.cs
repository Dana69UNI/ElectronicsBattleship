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

    public int sensor;
	private int Fase=0; //Fase 0 = juego no comenzado Fase 1 = Placement 1, Fase 2 Placement 2 ...
    SerialPort arduinoSerial = new SerialPort("COM9", 9600);

    void Start()
	{
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
			Fase = 4;
            //antes ataque 1
        }
        if (str == "121212")
        {
            Fase = 5;
            //despues ataque 1 / antes ataque 1
        }
        if (str == "252525")
        {
            Fase = 6;
            //antes ataque 2
        }
        if (str == "212121")
        {
            Fase = 7;
            //despues ataque 2 / antes ataque 1
        }
        if (str == "555")
        {
            //despues Win
        }
        if (str == "444")
        {
            //despues Water
        }
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
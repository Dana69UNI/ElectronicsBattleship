#include <FastLED.h>
#include <Asynchrony.h>

#define DATA_PIN 12
#define NUM_LEDS 8

const uint8_t SweepSize = NUM_LEDS;
CRGB leds[NUM_LEDS];

int trigPinShip = 10;
int echoPinShip = 9;
int trigPinFire = 6;
int echoPinFire = 5;
int ArcadeButtonShip=7;
int ArcadeLEDShip=8;
int ArcadeButtonFire=2;
int ArcadeLEDFire=3;
int LEDBUILT = 13;
int TiraLED = 12;
long time;
float distance;
float distanceBack = 22222;
float Ship1Placement;
float Ship2Placement;
float Player1Attack;
float Player2Attack;
bool winCon = true;

void setup()
{
  FastLED.setBrightness(15 );
   FastLED.addLeds<NEOPIXEL, DATA_PIN>(leds, NUM_LEDS);
   Asynchrony::schedule(1000, LEDPattern);
  pinMode(trigPinShip, OUTPUT); // SETTING OUTPUT PIN
  pinMode(echoPinShip, INPUT); // SETTING INPUT PIN
  pinMode(trigPinFire, OUTPUT); // SETTING OUTPUT PIN
  pinMode(echoPinFire, INPUT); 
  pinMode(TiraLED, OUTPUT);
  pinMode(ArcadeButtonShip, INPUT_PULLUP);
  pinMode(ArcadeLEDShip, OUTPUT);
  pinMode(ArcadeButtonFire, INPUT_PULLUP);
  pinMode(ArcadeLEDFire, OUTPUT);
  Serial.begin(9600); // INITIALISING THE COMMUNICATION
  pinMode(LEDBUILT, OUTPUT);
}

void loop()
{
   
  while(digitalRead(ArcadeButtonFire)==1)
  {
  digitalWrite(ArcadeLEDFire, 1);
  }
  digitalWrite(ArcadeLEDFire, 0);
  winCon = false;
  PlaceShip1();
  delay(4000);
  PlaceShip2();
  delay(4000);
  while(winCon == false)
  {
    delay(4000);
    Attack1();
    CheckAttackDMG1();
    if(winCon==true)
    {
      break;
    }
    delay(4000);
    Attack2();
    CheckAttackDMG2();
  }
}

  void PlaceShip1()
  {
    Serial.println(999);
   digitalWrite(ArcadeLEDShip, 0);
   FastLED.clear();
   LEDPattern();
    while(digitalRead(ArcadeButtonShip)==1)
    {
      
      
      
      distanceBack = distance;
      digitalWrite(trigPinShip,LOW);
      delayMicroseconds(2);
      // transmitting sound for 10 microseconds
      digitalWrite(trigPinShip, HIGH);
      delayMicroseconds(10);
      digitalWrite(trigPinShip, LOW);
      
      // calculating distance
      time=pulseIn(echoPinShip , HIGH);
      distance = time * 0.0343/2;
      SliderDistance(distance);
      //Serial.println(distance);
    }
    FastLED.clear();
    digitalWrite(ArcadeLEDShip, 0);
    Serial.println(010101);
    if(distance == 0)
    {
      Ship1Placement = distanceBack;
    }
    else
    {
      Ship1Placement = distance;
    }
    
    Serial.println(Ship1Placement);
  }

 void PlaceShip2()
  {
    Serial.println(666);
     digitalWrite(ArcadeLEDShip, 0);
   FastLED.clear();
   LEDPattern2();
    while(digitalRead(ArcadeButtonShip)==1)
    {
      
      distanceBack = distance;
      digitalWrite(trigPinShip,LOW);
      delayMicroseconds(2);

      // transmitting sound for 10 microseconds
      digitalWrite(trigPinShip, HIGH);
      delayMicroseconds(10);
      digitalWrite(trigPinShip, LOW);

      // calculating distance
      time=pulseIn(echoPinShip , HIGH);
      distance = time * 0.0343/2;
      Serial.println(distance);
      SliderDistance(distance);
  
    }
    FastLED.clear();
    FastLED.show();
    digitalWrite(ArcadeLEDShip, 0);
    Serial.println(020202);
    if(distance == 0)
    {
      Ship2Placement = distanceBack;
    }
    else
    {
      Ship2Placement = distance;
    }
    
    Serial.println(Ship2Placement);
  }

void Attack1()
  {
    Serial.println(151515);
    digitalWrite(ArcadeLEDFire, 1);
    while(digitalRead(ArcadeButtonFire)==1)
    {
      distanceBack = distance;
      digitalWrite(trigPinFire,LOW);
      delayMicroseconds(2);

      // transmitting sound for 10 microseconds
      digitalWrite(trigPinFire, HIGH);
      delayMicroseconds(10);
      digitalWrite(10, LOW);

      // calculating distance
      time=pulseIn(echoPinFire , HIGH);
      distance = time * 0.0343/2;
      SliderDistance(distance);
      //Serial.println(distance);
    }
   	digitalWrite(ArcadeLEDFire, 0);
     Serial.println(121212);
    if(distance == 0)
    {
      Player1Attack = distanceBack;
    }
    else
    {
      Player1Attack = distance;
    }
  }

void Attack2()
  {
    Serial.println(252525);
    digitalWrite(ArcadeLEDFire, 1);
    while(digitalRead(ArcadeButtonFire)==1)
    {
      
      distanceBack = distance;
      digitalWrite(trigPinFire,LOW);
      delayMicroseconds(2);

      // transmitting sound for 10 microseconds
      digitalWrite(trigPinFire, HIGH);
      delayMicroseconds(10);
      digitalWrite(10, LOW);

      // calculating distance
      time=pulseIn(echoPinFire , HIGH);
      distance = time * 0.0343/2;
      SliderDistance(distance);
      //Serial.println(distance);
    }
     Serial.println(212121);
    Serial.println("apretat");
   	digitalWrite(ArcadeLEDFire, 0);
     if(distance == 0)
    {
      Player2Attack = distanceBack;
    }
    else
    {
      Player2Attack = distance;
    }
  }

void CheckAttackDMG1()
{
  if(Player1Attack < Ship2Placement+5 && Player1Attack > Ship2Placement-5)
  {
    //hit
    Serial.println(999999);
    win();
  }
  else
  {
    water();
  }
}

void CheckAttackDMG2()
{
  if(Player2Attack < Ship1Placement+5 && Player2Attack > Ship1Placement-5)
  {
    //hit
    Serial.println("Hit");
    win();
  }
  else
  {
    water();
  }
}

void win()
{
  Serial.println("555");
  winCon = true;
  delay(10000);
}

void water()
{
  Serial.println("444");
  delay(5000);
}

void LEDPattern()
{
  
   for(int i = 0; i <= NUM_LEDS; i++)
      {
        fill_solid(leds, i, CRGB::White);
        FastLED.show();
        delay(500);
      }
    
      digitalWrite(ArcadeLEDShip, 1);
      
  
  
}

void LEDPattern2()
{
  
   
    for(int i = 0; i <= NUM_LEDS; i++)
      {
        fill_solid(leds, i, CRGB::White);
        FastLED.show();
        delay(500);
      }
      digitalWrite(ArcadeLEDShip, 1);
      
  
  
}

void SliderDistance(int dist)
{
  if(dist == 0)
  {
    
  }
  if(dist < 5)
  {
    Serial.println("3205");
  }
  if(dist >= 5 && dist < 10)
  {
    Serial.println("3210");
  }
  if(dist >= 10 && dist < 15)
  {
    Serial.println("3215");
  }
  if(dist >= 15 && dist < 20)
  {
    Serial.println("3220");
  }
  if(dist >= 20 && dist < 25)
  {
    Serial.println("3225");
  }
   if(dist >= 25 && dist < 30)
  {
    Serial.println("3230");
  }
   if(dist >= 30 && dist < 35)
  {
    Serial.println("3235");
  }
     if(dist >= 35 && dist < 40)
  {
    Serial.println("3240");
  }
     if(dist >= 40 && dist < 45)
  {
    Serial.println("3245");
  }
}

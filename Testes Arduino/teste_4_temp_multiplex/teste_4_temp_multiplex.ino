#include <Adafruit_MLX90614.h>
#include <Wire.h>

Adafruit_MLX90614 mlx[4];
int MultiplexPinA = 22, MultiplexPinB = 23;
bool _Primeiro = true;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  //Serial.println("Adafruit MLX90614 test");

  pinMode(MultiplexPinA, OUTPUT);
  pinMode(MultiplexPinB, OUTPUT);
  
  for(int i = 0; i < 4; i++)
  {
    switch(i) // seleciona qual dos 4 está selecionado pelo multiplex
    {
      case 0:
              digitalWrite(MultiplexPinA, LOW);
              digitalWrite(MultiplexPinB, LOW);
              break;
      case 1:        
              digitalWrite(MultiplexPinA, HIGH);
              digitalWrite(MultiplexPinB, LOW);
              break;   
      case 2:
              digitalWrite(MultiplexPinA, LOW);
              digitalWrite(MultiplexPinB, HIGH);
              break;         
      case 3:
              digitalWrite(MultiplexPinA, HIGH);
              digitalWrite(MultiplexPinB, HIGH);
              break;         
    }  
     
    mlx[i] = Adafruit_MLX90614();
    mlx[i].begin(); 
  }
  _Primeiro = true;
}

void loop() {
  Serial.print("_");
  
  for(int i = 0; i < 4; i++)
  {
    switch(i) // seleciona qual dos 4 está selecionado pelo multiplex
    {
      case 0:
              digitalWrite(MultiplexPinA, LOW);
              digitalWrite(MultiplexPinB, LOW);
              break;
      case 1:        
              digitalWrite(MultiplexPinA, HIGH);
              digitalWrite(MultiplexPinB, LOW);
              break;   
      case 2:
              digitalWrite(MultiplexPinA, LOW);
              digitalWrite(MultiplexPinB, HIGH);
              break;         
      case 3:
              digitalWrite(MultiplexPinA, HIGH);
              digitalWrite(MultiplexPinB, HIGH);
              break;         
    }
  // put your main code here, to run repeatedly:
    Serial.print(":"); 
    Serial.print(mlx[i].readObjectTempC());
    //Serial.print(":");
    //Serial.print(i);//Serial.println("*C");
  }
  Serial.println();
  delay(200);
}

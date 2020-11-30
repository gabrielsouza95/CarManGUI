//Current code by Electromania for Arduino Due compatibility
// https://youtu.be/sWjd61ouRVY  check this video for demo
//original code by Crenn from http://thebestcasescenario.com
//original project by Charles Gantt from http://themakersworkbench.com
//To disable global interrupts: cli(); for non DUE, and for DUE nointerrupts();
//to enable interrupts: sei(); for non DUE, for DUE Interrupt pins, 
// while on other boards only specific pins can be used as interrupt pins
// Refer- https://www.arduino.cc/en/Reference/AttachInterrupt
// for this project pin 12 has been used as interrupt input pin connected to halleffect sensor of fan motor.

#include <Wire.h>

LiquidCrystal_I2C //(0x27,16,2); // Constructor for LCD, (Address of LCD, NoOfColumns,NoOfRows)

//Varibles used for calculations
int ticks = 0, Speed = 0;

int hallsensor = 12;  //The Hall effect sensor (HES) output of fan  connected to pin no 12 of Arduino due

//Defines the structure for multiple fans and their dividers
typedef struct{   

char fantype;
unsigned int fandiv; 

}  fanspec;

//Definitions of the fans
//This is the varible used to select the fan and it's divider,
//set 1 for unipole hall effect sensor
//and 2 for bipole hall effect sensor
fanspec fanspace[3]={{0,1},{1,2},{2,8}}; char fan = 1;

void pickrpm ()
//This is the interrupt subroutine that increments ticks counts for each HES response.
{ ticks++; }

//This is the setup function where the serial port is initialised,
//and the interrupt is attached and other pins initialized.
void setup()
{
pinMode(hallsensor, INPUT);
Serial.begin(9600);
attachInterrupt(12, pickrpm, RISING); 
analogWriteResolution(12);

//.init();             // initialize the lcd 
//.backlight();         // backlight of lcd 
//.home();              // initialize lcd cursor position 
 
 pinMode(9, OUTPUT);
 pinMode(10, OUTPUT);
 pinMode(11, OUTPUT);

 digitalWrite(9, LOW);
 digitalWrite(10, LOW);
 digitalWrite(11, LOW);
}
  
void loop ()
{ 
  
ticks = 0;      // Make ticks zero before starting interrupts.

interrupts();    // or use sei(); to Enables interrupts
delay (1000);     //Wait 1 second
noInterrupts();  //  or use  cli();  to Disable interrupts

//Times sensorclicks (which is apprioxiamately the fequency the fan 
//is spinning at) by 60 seconds before dividing by the fan's divider
// this gives reasonable accuracy in upto few 10s of RPM
// for more accurate and fast measurements, other algorithms is required.
Speed = ((ticks * 60)/fanspace[fan].fandiv);  

//Print calculated Speed at the serial port 
  Serial.print (Speed, DEC);    
  Serial.print (" RPM\r\n");

//Display calculated Speed on LCD display

  //.setCursor(2, 0); // (column,row)
  //.print("Electromania");
  //.setCursor(0, 1); // (column,row)
  //.print("Speed:");

// Converting integer Speed to an ASCII string of 4 characters padded left
  char  SpeedString[4];    // Buffer to store string of 4 chars + 0 termination
  sprintf(SpeedString, "%4d", Speed);   // change this to %3, 4 ,5 etc depending upon your max speed
  //.print(SpeedString); 
  //.print(" RPM");

// Following code is for RGB LED.
  if (Speed > 3000){
    digitalWrite(9, HIGH);
    digitalWrite(10, LOW);
    digitalWrite(11, LOW);
  }
else if (Speed > 2000 && Speed < 3000){
    digitalWrite(10, HIGH);
    digitalWrite(9, LOW);
    digitalWrite(11, LOW);
  }

else if (Speed > 1000 && Speed < 2000){
    digitalWrite(11, HIGH); 
    digitalWrite(9, LOW);  //red
    digitalWrite(10, LOW);
  }
 if (Speed > 0 && Speed < 1000){
    digitalWrite(9, LOW);  //red
    digitalWrite(10, HIGH );
    digitalWrite(11, HIGH);
  }

   if (Speed == 0){
    digitalWrite(9, LOW);
    digitalWrite(10, LOW);
    digitalWrite(11, LOW);
  }

}  // end of loop function

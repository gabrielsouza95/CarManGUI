// digital pin 2 is the hall pin
const short hall_pin = 3;
// set number of hall trips for RPM reading (higher improves accuracy)
unsigned int time_before = 0, time_after = 0;
volatile unsigned int pulses = 0;

volatile float rpmcount = 0;
float rpm = 0;
float radial_vel = 0;
float linear_vel = 0;
unsigned long lastmillis = 0;


void setup() {
  Serial.begin(9600);
  attachInterrupt(digitalPinToInterrupt(hall_pin), rpm_bike, RISING); //interrupt zero (0) is on pin two(2).
}

// the loop routine runs over and over again forever:
void loop() {
  if (millis() - lastmillis >= 250)
  {  //Uptade every one second, this will be equal to reading frecuency (Hz).
    detachInterrupt(digitalPinToInterrupt(hall_pin));    //Disable interrupt when calculating.
    rpm = rpmcount * 240;  //Convert frecuency to RPM, note: this works for one interruption per full rotation. For two interrups per full rotation use rpmcount * 30.
    radial_vel = rpm * M_PI / 30; //convert rpm to radial velocity in rad/s.
    linear_vel = radial_vel * 0.33; //convert radial velocity to linear velocity in m/s.
    Serial.print("RPM = "); //print the word "RPM".
    Serial.print(rpm); // print the rpm value.
    Serial.print("\t\t Linear Speed = "); //print the word "Linear Velocity".
    Serial.print(linear_vel); //print the linear velocity value.
    Serial.println(" m/s"); //print the word "m/s".
   
    rpmcount = 0; // Restart the RPM counter
    lastmillis = millis(); // Update lasmillis
    attachInterrupt(digitalPinToInterrupt(hall_pin), rpm_bike, RISING); //enable interrupt
  }
}

//void change_state(){
//  pulses++;
//}

void rpm_bike()
{ // this code will be executed every time the interrupt 0 (pin2) gets low.
  rpmcount++;
}

// digital pin 2 is the hall pin
const short hall_pin = 3;
// set number of hall trips for RPM reading (higher improves accuracy)
unsigned int time_before = 0, time_after = 0;
volatile unsigned int pulses = 0;
bool state = true, passed_1seg = false;

void setup() {
  // initialize serial communication at 9600 bits per second:
  Serial.begin(9600);
  // make the hall pin an input:
  pinMode(hall_pin, INPUT);
  //attachInterrupt(digitalPinToInterrupt(hall_pin),change_state, RISING); 
}

// the loop routine runs over and over again forever:
void loop() {
//  if (state)
//  {
//    time_after = millis();
//    Serial.println(time_after - time_now);
//  }
//  else{
//    if (time_after < time_now)
//      time_now = millis();
//    else
//      time_now = time_after;
//  }
//  if(state)
//  {
//    time_after = millis();
//    time_before = millis();
//    while((time_before - time_after) >= 60000)
//      time_before = millis();
//    state = false;  
//  }
//  else
//  {
//    Serial.println(pulses);
//    pulses = 0;
//  }
  if(digitalRead(hall_pin))
  {
    time_after = millis();
    Serial.println(time_after - time_before);
  }
  else
  {
    time_before = millis();
    delay(1);
  }
}

//void change_state(){
//  pulses++;
//}

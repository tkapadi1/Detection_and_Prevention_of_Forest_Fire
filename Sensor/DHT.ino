// ESP8266 or Arduinio reading multiple DHT (11,21,22) Temperature and Humidity Sensors
// (c) D L Bird 2016
#include <ESP8266WiFi.h>

const char* ssid="Oneplus 7T";
const char* password = "Oneplus_7T";

#include "DHT.h"   //https://github.com/adafruit/DHT-sensor-library 
// Create the DHT temperature and humidity sensor object
#define DHTPIN3  D1 // DHT-1 sensor attached to pin 1
#define DHTTYPE3 DHT11
#define DHTPIN3  D2 // DHT-2 sensor attached to pin 2
#define DHTTYPE3 DHT11
#define DHTPIN3  D3// DHT-3 sensor attached to pin 3
#define DHTTYPE3 DHT11
#define TMPPin A0 //TMP36 attached to ESP8266 ESP-12E ADC

DHT dht1(D1, DHT11);
DHT dht2(D2, DHT11);
DHT dht3(D3, DHT11);
String temperatureString = "";      //variable to hold the temperature reading

void setup(void) {
  Serial.begin(9600);
  Serial.println("Wifi connecting to ");
  Serial.println( ssid );

  WiFi.begin(ssid,password);

  Serial.println();
  Serial.print("Connecting");

  while( WiFi.status() != WL_CONNECTED ){
      delay(500);
      Serial.print(".");        
  }
  Serial.println("Wifi Connected Success!");
  Serial.print("NodeMCU IP Address : ");
  Serial.println(WiFi.localIP() );

  dht1.begin();
  dht2.begin();
  dht3.begin();

}

void loop() {
  // Read DHT temperature and humidity values
  float DHT11_t = dht1.readTemperature() + 5;
  float DHT11_h = dht1.readHumidity();
  float f1 = (DHT11_t * 1.8) + 32;
  float t1 = f1;
  bool state1;
  if(f1 > 95 && f1 > 120){
    state1 = 2;
  }
  else if(f1 > 95){
    state1 = true;
  }
  else{
    state1 = false;
  }
   
  float DHT11_t1 = dht2.readTemperature() + 5;
  float DHT11_h1 = dht2.readHumidity();
  float f2 = (DHT11_t1 * 1.8) + 32;
  float t2 = f2;
  bool state2;
  if(f2 > 95 && f2 > 120){
    state2 = 2;
  }
  else if(f2 > 95){
    state2 = true;
  }
  else{
    state2 = false;
  }
  
  float DHT11_t2 = dht3.readTemperature() + 5;
  float DHT11_h2 = dht3.readHumidity();
  float f3 = (DHT11_t2 * 1.8) + 32;
  float t3 = f3;
  bool state3;
  if(f3 > 95 && f3 > 120){
    state3 = 2;
  }
  else if(f3 > 95){
    state3 = true;
  }
  else{
    state3 = false;
  }

  //analog TMP reading 
   
  int tmpValue = analogRead(TMPPin);
  float voltage = tmpValue * 3.3;// converting that reading to voltage
  voltage /= 1024.0;
  float temperatureC = (voltage - 0.5) * 100 ;  //converting from 10 mv per degree wit 500 mV offset
  //to degrees ((voltage - 500mV) times 100)
  float temperatureF = (temperatureC * 9.0 / 5.0) + 32.0;  //now convert to Fahrenheit
  float t4 = temperatureF;
  bool state4;
  if(temperatureF > 95 && temperatureF > 120){
    state4 = 2;
  }
  else if(temperatureF > 95){
    state4 = true;
  }
  else{
    state4 = false;
  }
  temperatureString = String(temperatureF) + ", 22," + state4;
  Serial.println(temperatureString);
  Serial.println();
  delay(200);


  //Digital DHT11 reading
  
  Serial.print(f1); Serial.print(", 15, "); Serial.println(state1);
  delay(200);

  Serial.print(f2); Serial.print(", 16, "); Serial.println(state2);
  delay(200);

  Serial.print(f3); Serial.print(", 21, "); Serial.println(state3);
  delay(200);
}

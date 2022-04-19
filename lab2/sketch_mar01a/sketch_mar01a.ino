#define DELAY (700)
#define FIRST_BUTTON_PIN (A8)
#define SECOND_BUTTON_PIN (A9)
#define LED_PINS_SIZE (8)

// Led configuration.
const int ledPinsNumbers[] = {42, 43, 44, 45, 46, 47, 48, 49};

void setup()
{
  // Pins configuration.
  for (auto& pinNumber : ledPinsNumbers)
  {
    pinMode(pinNumber, OUTPUT);
  }

  // Buttons configuration.
  pinMode(FIRST_BUTTON_PIN, INPUT_PULLUP);
  pinMode(SECOND_BUTTON_PIN, INPUT_PULLUP);

  // Virtual device configuration.
  Serial.begin(9600);
}

void loop()
{
  // Buttons handling.
  if (Serial.available())
  {
    int serialInput = Serial.read();
    
    if (serialInput == 0xA1)
    {
      for (int i = 0; i < LED_PINS_SIZE / 2; i++)
      {
        digitalWrite(ledPinsNumbers[i], HIGH);
        digitalWrite(ledPinsNumbers[LED_PINS_SIZE - i - 1], HIGH);
        delay(DELAY);
        digitalWrite(ledPinsNumbers[i], LOW);
        digitalWrite(ledPinsNumbers[LED_PINS_SIZE - i - 1], LOW);
      }
    }
    else if (serialInput == 0xB1)
    {
      for (int i = 0; i < LED_PINS_SIZE; i += 2)
      {
        digitalWrite(ledPinsNumbers[i], HIGH);
        delay(DELAY);
        digitalWrite(ledPinsNumbers[i], LOW);
      }
      for (int i = 1; i < LED_PINS_SIZE; i += 2)
      {
        digitalWrite(ledPinsNumbers[i], HIGH);
        delay(DELAY);
        digitalWrite(ledPinsNumbers[i], LOW);
      }
    }
    
  }

  if (digitalRead(FIRST_BUTTON_PIN) == LOW)
  {
    Serial.write(0xA1);
  }
  
  if(digitalRead(SECOND_BUTTON_PIN) == LOW)
  {
    Serial.write(0xB1);
  }
}

#define BUTTON_PIN (A8)
#define LED_PIN_SIZE (8)

int ledPins[LED_PIN_SIZE];
int isButtonActivated = 0;

void setup() {
  for(int i = 0; i < LED_PIN_SIZE; i++)
  {
    ledPins[i] = 42 + i;
    pinMode(ledPins[i], OUTPUT);
  }
  pinMode(BUTTON_PIN, INPUT);
}

void loop() {
  const int deley = 500;
  if (digitalRead(BUTTON_PIN) == 1)
  {
    isButtonActivated = 1;
  }
  
  if (isButtonActivated && digitalRead(BUTTON_PIN) == 0)
  {
    for (int i = 0; i < LED_PIN_SIZE / 2; i++)
    {
      digitalWrite(ledPins[i], HIGH);
      digitalWrite(ledPins[LED_PIN_SIZE - 1 - i], HIGH);
      delay(deley);
      digitalWrite(ledPins[i], LOW);
      digitalWrite(ledPins[LED_PIN_SIZE - 1 - i], LOW);
    }
    isButtonActivated = 0;
  }
}

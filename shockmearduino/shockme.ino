int speakerPin = 13;

int length = 15; // the number of notes
char notes[] = "dddCedCaaagfga "; // a space represents a rest
int beats[] = { 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 2, 4 };
int tempo = 500;

void playTone(int tone, int duration) {
  for (long i = 0; i < duration * 1000L; i += tone * 2) {
    digitalWrite(speakerPin, HIGH);
    delayMicroseconds(tone);
    digitalWrite(speakerPin, LOW);
    delayMicroseconds(tone);
  }
}

void playNote(char note, int duration) {
  char names[] = { 'c', 'd', 'e', 'f', 'g', 'a', 'b', 'C','D','E' };
  int tones[] = { 1915, 1700, 1519, 1432, 1275, 1136, 1014, 956,851,758 };

  // play the tone corresponding to the note name
  for (int i = 0; i < 8; i++) {
    if (names[i] == note) {
      playTone(tones[i], duration);
    }
  }
}

void setup() {
  Serial.begin(9600);
  pinMode(speakerPin, OUTPUT);
}

void loop() {
 if (Serial.available() > 0) {
                // read the incoming byte:
                String incomingByte = Serial.readString();

                // say what you got:
                //Serial.print("I received: ");
                //Serial.println(incomingByte);
                if(incomingByte == "pm")
                {
                  //Serial.println("IN");
                  for (int i = 0; i < length; i++) {
                    if (notes[i] == ' ') {
                      delay(beats[i] * tempo); // rest
                    } else {
                      playNote(notes[i], beats[i] * tempo);
                    }
                
                    // pause between notes
                    delay(tempo / 2); 
                  }
                }
  }
  
}

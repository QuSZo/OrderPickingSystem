#!/usr/bin/python
# -*- coding:utf-8 -*-
import RPi.GPIO as GPIO
from AlphaBot2 import AlphaBot2
from rpi_ws281x import Adafruit_NeoPixel, Color
from TRSensors import TRSensor
import time
import random

Button = 7
BUZ = 4

def beep_on():
	GPIO.output(BUZ,GPIO.HIGH)
def beep_off():
	GPIO.output(BUZ,GPIO.LOW)
"""
def generuj_instrukcje(lista):
    instrukcje = []
    poprzedni_A = False  # czy poprzedni element był A

    for i, x in enumerate(lista):
        # przypadek A
        if x == "A":
            instrukcje.append("left")
            poprzedni_A = True
		elif else
			instrukcje.append("forward")

		# przypadek B
		elif x == "B":
			if poprzedni_A:
				instrukcje.append("right")
			else:
				instrukcje.append("left")

		# przypadek inny niż A i B
		else:
			if poprzedni_A:
				instrukcje.append("forward")
			else:
				instrukcje.append("forward")
			poprzedni_A = False

    return instrukcje
"""

GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)
GPIO.setup(Button,GPIO.IN,GPIO.PUD_UP)
GPIO.setup(BUZ,GPIO.OUT)

# LED strip configuration:
LED_COUNT      = 4      # Number of LED pixels.
LED_PIN        = 18      # GPIO pin connected to the pixels (must support PWM!).
LED_FREQ_HZ    = 800000  # LED signal frequency in hertz (usually 800khz)
LED_DMA        = 5       # DMA channel to use for generating signal (try 5)
LED_BRIGHTNESS = 255     # Set to 0 for darkest and 255 for brightest
LED_INVERT     = False   # True to invert the signal (when using NPN transistor level shift)	

maximum = 60
isFastRun = True
j = 0
integral = 0
last_proportional = 0
previousOnTransverseLine = False

#lista = ["A","C"]
#instrukcje = generuj_instrukcje(lista)
#instrukcje = ['forward', 'forward', 'forward', 'forward', 'forward', 'forward', 'forward', 'forward', 'forward',
#'forward', 'forward', 'right', 'forward', 'right', 'forward', 'forward', 'forward', 'forward', 'forward', 'forward',
#'forward', 'forward', 'forward', 'forward', 'forward', 'right']

instrukcje = ['right', 'forward', 'forward', 'forward', 'forward', 'forward', 'right', 'left',
'forward', 'forward', 'forward', 'forward', 'forward', 'right', 'right',
'forward', 'forward', 'forward', 'forward', 'forward', 'right', 'left',
'forward', 'forward', 'forward', 'forward', 'forward', 'right'
]

def Countdown():
	for x in range(1, -1, -1):
		print(x)
		beep_on()
		time.sleep(0.5)
		beep_off()
		time.sleep(0.5)

def Wheel(pos):
#	"""Generate rainbow colors across 0-255 positions."""
	if pos < 85:
		return Color(pos * 3, 255 - pos * 3, 0)
	elif pos < 170:
		pos -= 85
		return Color(255 - pos * 3, 0, pos * 3)
	else:
		pos -= 170
		return Color(0, pos * 3, 255 - pos * 3)

# Create NeoPixel object with appropriate configuration.
strip = Adafruit_NeoPixel(LED_COUNT, LED_PIN, LED_FREQ_HZ, LED_DMA, LED_INVERT, LED_BRIGHTNESS)
# Intialize the library (must be called once before other functions).
strip.begin()
strip.setPixelColor(0, Color(100, 0, 0))       #Red
strip.setPixelColor(1, Color(0, 100, 0))       #Blue
strip.setPixelColor(2, Color(0, 0, 100))       #Green
strip.setPixelColor(3, Color(100, 100, 0))     #Yellow
strip.show()

TR = TRSensor()
Ab = AlphaBot2()
Ab.stop()

print("Line follow Example")
time.sleep(0.5)
for i in range(-4,80):
	if(i<20 or i>= 60):
		Ab.right()	
		Ab.setPWMA(40)
		Ab.setPWMB(40)
	else:
		Ab.left()
		Ab.setPWMA(40)
		Ab.setPWMB(40)
	TR.calibrate()
Ab.stop()
print(TR.calibratedMin)
print(TR.calibratedMax)
#while (GPIO.input(Button) != 0):
#	position,Sensors = TR.readLine()
#	print(position,Sensors)
#	time.sleep(0.05)

print(instrukcje)
Countdown()

Ab.forward()

while True:
	try:
		#position,Sensors,onTransverseLine = TR.readLine()
		position,Sensors = TR.readLine()
		#print(position)
		#print(Sensors[0], Sensors[1], Sensors[2], Sensors[3], Sensors[4])
		if(Sensors[0] >600 and Sensors[1] >600 and Sensors[2] >600 and Sensors[3] >600 and Sensors[4] >600):
			print("Skrzyżowanie - wybieram kierunek!")
			# Wybierz kierunek (tu np. losowo)
			#direction = random.choice(["left", "right"])
			#direction = "left"
			direction = instrukcje.pop(0)
			if direction == "left":
				print("Turn: " + direction)
				Ab.left()
				time.sleep(0.6)
			elif direction == "right":
				print("Turn: " + direction)
				Ab.right()
				time.sleep(0.6)
			elif direction == "forward":
				print("Turn: " + direction)
				Ab.setPWMA(maximum)
				Ab.setPWMB(maximum)
				Ab.forward()
				time.sleep(0.2)
			else:
				break
			Ab.forward()
			time.sleep(0.2)
		else:
			# Robot on transverse line
			#if(onTransverseLine == True and previousOnTransverseLine == False):
			#	if(isFastRun):
			#		maximum = 60
			#		isFastRun = False
			#	else:
			#		maximum = 50
			#		isFastRun = True

			#previousOnTransverseLine = onTransverseLine

			# The "proportional" term should be 0 when we are on the line.
			proportional = position - 2000
			
			# Compute the derivative (change) and integral (sum) of the position.
			derivative = proportional - last_proportional
			integral += proportional
			
			# Remember the last position.
			last_proportional = proportional

			'''
			// Compute the difference between the two motor power settings,
			// m1 - m2.  If this is a positive number the robot will turn
			// to the right.  If it is a negative number, the robot will
			// turn to the left, and the magnitude of the number determines
			// the sharpness of the turn.  You can adjust the constants by which
			// the proportional, integral, and derivative terms are multiplied to
			// improve performance.
			'''
			power_difference = proportional/30  + integral/10000 + derivative*2;  

			if (power_difference > maximum):
				power_difference = maximum
			if (power_difference < - maximum):
				power_difference = - maximum
			#print(position,power_difference)
			if (power_difference < 0):
				Ab.setPWMA(maximum + power_difference)
				Ab.setPWMB(maximum)
			else:
				Ab.setPWMA(maximum)
				Ab.setPWMB(maximum - power_difference)
			
		for i in range(0,strip.numPixels()):
			strip.setPixelColor(i, Wheel((int(i * 256 / strip.numPixels()) + j) & 255))
		strip.show()
		j += 1
		if(j > 256*4): 
			j= 0
	except KeyboardInterrupt:
		break

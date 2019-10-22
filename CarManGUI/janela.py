from Tkinter import *
import tkFont
import serial

# win = Tk()

# myFont = tkFont.Font(family = 'Microsoft Sans Serif', size = 8, weight = 'normal')

NORMAL_FONT = ("Microsoft Sans Serif", 12)

class TesteJanelaTKinter(Tkinter.Tk):
	def __init__(self, *args, **kwargs):
		Tkinter.Tk.__init__(self, *args, **kwargs)
		container = Tkinter.Frame(self)

		container.pack(side ="top", fill = "both", expand = true)

		container.grid_rowconfigure(0, weight = 1)
		container.grid_columnconfigure(0, weight = 1)

		self.frames = {}

		frame = StartPage(container, self)

		self.frames[StartPage] = frame

		frame.grid(row = 0, column = 0, sticky = "nsew")

		frame.grid(row = 20, column = 20, sticky = "nsew")

		self.show_frame(StartPage)

	def show_frame(self, cont):
		frame = self.frames[cont]
		frame.tkraise()	
		
class StartPage(Tkinter.Frame):
	def __init__(self, parent, controller):
		Tkinter.Frame.__init__(self, parent)
		label = Tkinter.Label(self, text = "Start Page", font = NORMAL_FONT)
		label.pack(pady = 10, padx = 10)

app = TesteJanelaTKinter();
app.mainloop()


  # def hello_world(self):
  #   print "hello_world"

  # def exitProgram()
  # 	win.quit()  

# win.title("KarManGUI")
# win.geometry('420x320')

# exitButton = Button(win, text = "Exit", font = myFont, comand = exitProgram, height = 2, width = 6)
# exitButton.pack(side = BOTTOM)
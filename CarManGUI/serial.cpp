
#include <fcntl.h>
#include <sys/ioctl.h>
#include <termios.h>
#include <unistd.h>
#include <fstream>
#include <iostream>
#include <string>

using namespace std;

struct termios cfg;

int fd = 0;

std::fstream file; //cria variável de arquivo para log

void erro(char *s) {
  perror(s);
  exit(3);
}

void iniciaComunicacao(char device[]) {
  fd = open(device, O_RDWR | O_NOCTTY);
  if (fd == -1) {
    erro("erro na abertura da interface serial\n");
  }

  if (!isatty(fd)) {
    erro("A interface serial aberta nao é realmente uma interface serial!\n");
  }

  if (tcgetattr(fd, &cfg) < 0) {
    erro("A configuracao da interface serial nao pode ser lida!\n");
  }

  cfg.c_iflag = IGNBRK | IGNPAR;
  cfg.c_oflag = 0;
  cfg.c_lflag = 0;
  cfg.c_cflag = B115200 | CS8; // | CRTSCTS;// | CSTOPB;
  cfg.c_ispeed = 115200;//9600;
  cfg.c_ospeed = 115200;//9600;

  cfg.c_cc[VMIN] = 1;
  cfg.c_cc[VTIME] = 0;

  //	int sercmd = 0;// desliga os bits TIOCM_RTS | TIOCM_DTR para colocar 12V
  // na saida 	ioctl(fd, TIOCMBIC, &sercmd); // Set the RTS pin.

  if (tcsetattr(fd, TCSAFLUSH, &cfg) < 0) {
    erro("A configuracao da interface serial nao pode ser alterada!\n");
  }

  if (cfsetispeed(&cfg, B500000) < 0 || cfsetospeed(&cfg, B500000) < 0) {
    erro("A interface serial nao pode ser configurada!\n");
  }
}

int recebeByte() {
  unsigned char c;
  if (read(fd, &c, 1) >= 0) {
    // printf("(%02x) ", c);
    return c;
  } else {
    return -1;
  }
}

string recebeString() {
  string s;
  unsigned char c;
  while (read(fd, &c, 1) >= 0) {
    if (c == '\n')
      break;
    s += c;
  }
  s+= "\n";
  return s;
}

void enviaByte(unsigned char b) {
  tcflush(fd, TCIOFLUSH);
  // unsigned char b[NUM_ROBOS_TIME*2 + 1];
  int resp = write(fd, &b, 1);
  if (resp < 0) {
    erro("Interface serial - nao pode enviar dados.\n");
  }
}

void terminaComunicacao(void) {
  int sercmd = TIOCM_RTS | TIOCM_DTR;
  ioctl(fd, TIOCMBIC, &sercmd);
  close(fd);
}

void check() {
    string text;
    //ifstream file;
    //file.open("testlog.txt", std::fstream::in | std::fstream::out | std::fstream::app);
    getline(file, text);

    if (text == "") {
        cout << "There's no data in file" << endl;
    } else {
        cout << "File contains:" << endl;
        cout << text << endl;
    }
}

int main(int argc, char *argv[]) {
  iniciaComunicacao("/dev/ttyACM0"); //lida com a serial
  
  std::fstream file; //cria variável de arquivo para log

  string s; //variável que vai receber as infos da serial
  int op = 0; //variável para controlar se abre o arquivo ou não
  file.open("testlog.txt", std::fstream::app | std::fstream::out); //abre arquivo para gravar std::fstream::in | std::fstream::out | 
    
  do {
    if(!file.is_open() && op == 0)
      file.open("testlog.txt", std::fstream::app | std::fstream::out);
    
    s = recebeString(); //recebe da serial
    cout << s << endl; //escreve no terminal o q recebeu da serial

    if(op == 0)
      file << s << std::flush; //escreve no arquivo o que recebeu da serial
    
    //printf("%s\n",&s);
    scanf("%s",&s); //recebe da janela pai o standard input

    if(s == "fecha" || (file.is_open() && op == 1)) //caso a janela mande a ordem para parar de gravar o arquivo ou
    {                                               //a janela já mandou mas o arquivo ainda está aberto
      op = 1;
      file.close();
    }
    if(s == "abre")
      op = 0;

  } while (s != "fim");

  //file.close();
  terminaComunicacao();

  return 0;
}

<H1> # CarManGUI 

<H3> O Projeto
Projeto está sendo desenvolvido em parceria com a FEB Racing, equipe de fórmula SAE da Unesp, para a construção de um equipamento para o carro da equipe.

Em resumo, ele consiste de:
  - sensores colocados no carro;
  - gravação dos dados coletados dos sensores em arquivo para posterior análise;
  - mostrar os dados para o piloto através de uma interface com o usuário e, futuramente, para a equipe nos boxes com possibilidade de comunicação simples entre a equipe e o piloto: - Por exemplo, mostrar para o piloto que foi dada bandeira de cor x.
  - um plotter para visualização dos dados e análise.
<H3> O Hardware  
O  hardware do projeto atualmente é constituido de:
  - 1 Arduino Mega;
  - 1 tela OLED de 0,96" que comunica por I2C;
  - 3 botões ligados com um pullup comum;
  - 1 Raspberry;
  - 1 tela touch 3,5", encaixa por cima do Raspberry, habilitada via raspibian;
  - 1 multiplexador 4052.
<H3> O Software
  
<H3> O que está acontecendo
- Atualmente o projeto está com uma janela desenvolvida em C# para fazer a interface com o usurário através da tela de 3,5" do Raspberry, fazendo a inicialização da comunicação e o término da mesma com o arduino; controla quando começa a gravar os dados, dando feedback que está gravando e sendo possível parar a gravação em qualquer momento*; mostra os dados dos sensores em telas separadas para uma melhor visualização.
A mesma abre no Raspberry mas por algum motivo, depois de mudar o método de utilizar uma thread manualmente, para utilizar eventos da porta serial do objeto serial do C#, ele não comunicou mais com o Arduino. Pelo que já descobri
  
<H3> Para implementar

<body>
<H1> # CarManGUI </H1>

<H3> O Projeto </H3>
 
<p>Projeto está sendo desenvolvido em parceria com a FEB Racing(<a href="https://www.facebook.com/equipefebracing">face</a>)(<a href="https://www.instagram.com/febracing/?hl=pt">insta</a>), equipe de fórmula SAE da Unesp, para a construção de um equipamento para o carro da equipe.</p>

<span><img style="max-width:50%; max-height:50%;" src="https://github.com/gabrielsouza95/CarManGUI/blob/master/primeiro_teste_no_carro.jpeg" alt="Primeiro teste no carro">
</span>

<p>Em resumo, ele consiste de:
 <ol>
  <li>sensores colocados no carro;</li>
  <li>gravação dos dados coletados dos sensores em arquivo para posterior análise;</li>
  <li>mostrar os dados para o piloto através de uma interface com o usuário e, futuramente, para a equipe nos boxes com possibilidade de comunicação simples entre a equipe e o piloto:<ol><li> - Por exemplo, mostrar para o piloto que foi dada bandeira de cor x.</li></ol></li>
  <li>um plotter para visualização dos dados e análise.</li>
 </ol>
</p>

<H3> O Hardware </H3> 
   
<p>O  hardware do projeto atualmente é constituido de:
  <li>1 Arduino Mega;</li>
  <li>1 tela OLED de 0,96" que comunica por I2C;</li>
  <li>3 botões ligados com um pullup comum;</li>
  <li>1 Raspberry;</li>
  <li>1 tela touch 3,5", encaixa por cima do Raspberry, habilitada via raspibian;</li>
  <li>1 multiplexador 4052.</li>
</p>  
  
<p>Está sendo implementado a adição de:
  <li>4 ATtiny85. </li>
</p>

<H3> O Software </H3>

<p>Na implementação do projeto está sendo utilizado C# para a interface utilizando um projeto .NET Framework 4 e o <a href="https://www.mono-project.com/docs/getting-started/install/linux/#debian-ubuntu-and-derivatives">Mono</a> e no futuro será utilizado o .NET Core 3 que tem compatibilidade com o Raspibian.</p>
<span><img style="max-width:50%; max-height:50%;" src="https://github.com/gabrielsouza95/CarManGUI/blob/master/teste_interface_csharpv2.x_animado.gif" alt="teste_interface_csharpv2.x_animado">
</span><figcaption>GIF mostra o teste da janela em C#, com um gráfico 2D implementado sendo printado os valores de 2 dos 3 eixos do acelerômetro no gráfico de pontos enquanto balanço o sensor.</figcaption>
<p>Python, R, C++(Arduino, Processing, tentativa de script para comunicação serial). </p>

<H3> O que está acontecendo </H3>


<p>- Atualmente o projeto está com uma janela desenvolvida em C# para fazer a interface com o usurário através da tela de 3,5" do Raspberry, fazendo a inicialização da comunicação e o término da mesma com o arduino; controla quando começa a gravar os dados, dando feedback que está gravando e sendo possível parar a gravação em qualquer momento*; mostra os dados dos sensores em telas separadas para uma melhor visualização.
A mesma abre no Raspberry mas por algum motivo, depois de mudar o método de utilizar uma thread manualmente, para utilizar eventos da porta serial do objeto serial do C#, ele não comunicou mais com o Arduino. Pelo que já descobri, o software de compatibilidade Mono que estou utilizando no Raspberry não tem implementado a compatibilidade dos eventos de porta serial.
 </p>
 
<H3> Para implementar </H3>
<p>- Desejo utilizar JonhyFive juntamente com Node para comunicar entre raspberry e arduino, além de estar estudando a possibilidade de fazer uma interface com React e já deixar pronto para ser utilizado via internet o acesso ao carro pelo computador nos boxes também com uma página web em React ou até mesmo um app no celular com React Native.
</p>
<p>- Como mencionado na parte de software, provavelmente antes da implementação com JS(JonhyFive,Node,React,ReactNative) será implementada uma versão utilizando o .NET Core 3.0 que é garantido a compatibilidade com o Raspibian, apesar de não ser a ferramenta mais nova.
</p>
</body>

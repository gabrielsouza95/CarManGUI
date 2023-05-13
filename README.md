# CarManGUI

### O Projeto
 
Projeto está sendo desenvolvido em parceria com a FEB Racing([facebook](https://www.facebook.com/equipefebracing))([instagram](https://www.instagram.com/febracing/?hl=pt)), equipe de fórmula SAE da Unesp, para a construção de um equipamento para o carro da equipe.

<span>
    <img style="max-width:50px; max-height:50px;" 
    src="https://github.com/gabrielsouza95/CarManGUI/blob/master/primeiro_teste_no_carro.jpeg" 
    alt="Primeiro teste no carro">
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
 <ol>
  <li>1 <a href="https://www.arduino.cc/en/Guide/ArduinoMega2560">Arduino Mega</a>;</li>
  <li>Alguns sensores, sendo eles:</li>
  <ol>
   <li>3 acelerômetros giroscópios <a href="https://www.letscontrolit.com/wiki/index.php/MPU6050">MPU6050</a></li>
   <li>4 sensores de temperatura por infra vermelho <a href="https://forum.arduino.cc/index.php?topic=577921.0">MLX90614</a></li>
   <li>4 sensores de efeito hall <a href="https://www.instructables.com/Arduino-Magnetic-FIELD-Detector-Using-the-KY-003-o/">KY003</a></li>
   <li>4 potênciometros para verificação de curso de suspensão</li>
   <li>1 sensor de pressão de linha de freio</li>
  </ol>
  <li>1 <a href="https://randomnerdtutorials.com/guide-for-oled-display-with-arduino/">tela OLED de 0,96"</a> que comunica por I2C;</li>
  <li>3 botões ligados com um pullup comum;</li>
  <li>1 <a href="https://circuitdigest.com/simple-raspberry-pi-projects-for-beginners">Raspberry</a>;</li>
  <li>1 <a href="https://www.youtube.com/watch?v=Fj3wq98pd20">tela touch 3,5"</a>, encaixa por cima do Raspberry, habilitada via raspibian;</li>
  <li>1 multiplexador 4052;</li>
  <li>1 adaptador <a href="https://www.electronicshub.org/arduino-mcp2515-can-bus-tutorial/">MCP2515</a>.</li>
 </ol>
</p>  
  
<p>Está sendo implementado a adição de:
  <li>4 <a href="https://thewanderingengineer.com/2014/02/17/attiny-i2c-slave/">ATtiny85</a>. </li>
</p>

<H3> O Software </H3>

<p>Na implementação do projeto está sendo utilizado C# para a interface utilizando um projeto .NET Framework 4 e o <a href="https://www.mono-project.com/docs/getting-started/install/linux/#debian-ubuntu-and-derivatives">Mono</a> e no futuro será utilizado o .NET Core 3 que tem compatibilidade com o Raspibian.</p>
<span><img style="max-width:50%; max-height:50%;" src="https://github.com/gabrielsouza95/CarManGUI/blob/master/teste_interface_csharpv2.x_animado.gif" alt="teste_interface_csharpv2.x_animado">
</span><figcaption>GIF mostra o teste da janela em C#, com um gráfico 2D implementado sendo printado os valores de 2 dos 3 eixos do acelerômetro no gráfico de pontos enquanto balanço o sensor.</figcaption>
<p> </p>
<p>Também está sendo utilizado no projeto Python, R, C++(Arduino, Processing, tentativa de script para comunicação serial). </p>

<H3> O que está acontecendo </H3>


<p>- Atualmente o projeto está com uma janela desenvolvida em C# para fazer a interface com o usurário através da tela de 3,5" do Raspberry, fazendo a inicialização da comunicação e o término da mesma com o arduino; controla quando começa a gravar os dados, dando feedback que está gravando e sendo possível parar a gravação em qualquer momento*; mostra os dados dos sensores em telas separadas para uma melhor visualização.
</p>

<p>
A mesma abre no Raspberry mas por algum motivo, depois de mudar o método de utilizar uma thread manualmente, para utilizar eventos da porta serial do objeto serial do C#, ele não comunicou mais com o Arduino. Pelo que já descobri, o software de compatibilidade Mono que estou utilizando no Raspberry não tem implementado a compatibilidade dos eventos de porta serial.
</p>

<p>- Como não funcionou no Raspberry corretamente a janela implementada em C#, estou utilizando uma tela OLED de 0,96" para mostrar ao piloto as informações desejadas, com 2 botões para a seleção do menu, um vai "para frente" nas opções e o outro volta. O terceiro botão é para controlar quando o Arduino deve começar a gravar ou não. Estaria utilizando um shield de cartão SD mas o mesmo não funcionou também
</p>

<p>
- Foi desenvolvida uma janela em Python para ser possível levar o equipamento funcionando para a competição, dado que o módulo SD não funcionou ligado diretamente no arduino. Com essa janela, já foi testada e o arduino consegue fazer conexão com ela e gerar o arquivo de log. Por conta da pandemia, ainda não foi possível testar o equipamento no carro da equipe mas testarei em meu Corsa e será postado aqui o resultado. 
Inclusive do teste antes e depois da instalação da barra estabilizadora, que na época, os carros populares vinham tão pelados que sequer esse item de segurança ou o protetor de cárter vinham no carro para baratea-lo.
</p> 

<p>
 - Foi feita uma base para facilitar os testes de bancada que estão sendo feitos atualmente, como pode ser visto nas duas próximas imagens:
</p>
 
<span><img style="max-width:50px; max-height:50px;" src="https://github.com/gabrielsouza95/CarManGUI/blob/master/base_teste_v1.1_view2.jpeg" alt="Vista frontal da base de testes">
</span>
<span><img style="max-width:50px; max-height:50px;" src="https://github.com/gabrielsouza95/CarManGUI/blob/master/base_teste_v1.1_view6.jpeg" alt="Vista superior da base de testes">
</span>
 
<p>
 - Foi feito o aviso de bandeiras que poderá ser dado dos boxes para informar o piloto de alguma condição que precisa de atenção. Com essa implementação também podem ser feitos outros avisos que sejam necessários serem passados para o piloto. Na imagem também é possível ver como era mostrada as informações do acelerômetro:
</p>
 
<span><img style="max-width:50px; max-height:50px;" src="https://github.com/gabrielsouza95/CarManGUI/blob/master/teste_alerta_bandeiras.gif" alt="Teste do alerta de bandeiras">
</span>
 
 <p>
 - Foi melhorado a interface dos dados do acelerômetro para que seja mais compreensível para o piloto. Foi escolhido os eixos de aceleração laterais e pra frente e para trás na implementação de como o ponto deve se deslocar. Me inspirei no display de força G do Gran Turismo para fazê-lo. Na imagem a seguir é possível ver a primeira versão:
</p>
 
<span><img style="max-width:50px; max-height:50px;" src="https://github.com/gabrielsouza95/CarManGUI/blob/master/teste_UI_acelerometro_1.0.gif" alt="Primeira versão da UI do acelerômetro">
</span>
 
 <p>
 - Nessa próxima imagem é possível ver a segunda versão da interface:
</p>
 
 <span><img style="max-width:50px; max-height:50px;" src="https://github.com/gabrielsouza95/CarManGUI/blob/master/teste_UI_acelerometro_1.1.gif" alt="Segunda versão da UI do acelerômetro">
</span>
 
 
<H3> Para implementar </H3>
<p>- Desejo utilizar JonhyFive juntamente com Node para comunicar entre raspberry e arduino, além de estar estudando a possibilidade de fazer uma interface com React e já deixar pronto para ser utilizado via internet o acesso ao carro pelo computador nos boxes também com uma página web em React ou até mesmo um app no celular com React Native.
</p>
<p>- Como mencionado na parte de software, provavelmente antes da implementação com JS(<a href="http://johnny-five.io/">JonhyFive</a>,<a href ="https://www.instructables.com/NodeJs-and-Arduino/">Node</a>,<a href="https://awot.net/en/guide/tutorial.html">React</a>,ReactNative) será implementada uma versão utilizando o .NET Core 3.0 que é garantido a compatibilidade com o Raspibian, apesar de não ser a ferramenta mais nova.
</p>
<p>- Os ATtiny85 que serão implementados, vão ser utilizados como sensores de velocidade, ficando entre o Arduino e o sensor hall, para fazer a correta contagem de pulsos por intervalo de tempo. Como o Aruino está hoje, caso tentássemos utilizar os 4 sensores hall diretamente neles, por conta de como é calculado a velocidade da roda por amostragem de pulsos por tempo, o Arduino ficaria com uma resposta muito ruim (<a href="https://forum.arduino.cc/index.php?topic=519300.0">veja aqui</a>), por utilizar rotinas de interrupção do processador para tal tarefa. Então a ideia é que o ATtiny85 fique ligado como um slave no barramento I2C (<a href="https://thewanderingengineer.com/2014/02/17/attiny-i2c-slave/">veja aqui</a>) e quando o Arduino solicitar, ele já envie a velocidade atual da roda, deixando o cálculo com interrupções diretamente no ATtyny85, que tem suporte para tal.
</p>
<p>- Estou estudando a possibilidade de migrar o projeto direto para um <a href="https://github.com/esp8266/Arduino">ESP8266</a> para utilizar não somente a função WiFi, em conjunto com o Raspberry, mas também para melhorar a velocidade do hardware responsável pela captura de dados dos sensores, assim tendo uma melhor amostragem dos dados dos sensores, também sendo possível adicionar mais captura de dados, como o adaptador CAN e mais sensores.
</p>
<p>- Quero incluir no equipamento a leitura do barramento CAN, que no caso estou tentando fazer funcionar com o adaptador <a href="https://www.electronicshub.org/arduino-mcp2515-can-bus-tutorial/">MCP2515</a> mas que, até o momento, não consegui fazer a inicialização do adaptador para processguir com a leitura dos dados da injeção eletrônica para ser associada ao log de dados dos sensores adicionados pelo projeto.
</p>
</body>

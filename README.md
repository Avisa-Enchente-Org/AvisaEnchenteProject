# Introdução

## Objetivo: 
Esse projeto tem como objetivo, ser uma **Aplicação Web MVC**, da qual será possivel vizualizar e monitorar em tempo real,
possiveis enchentes ou alagamentos em regiões definidas. 


## Como funciona?
Essa aplicação se conectará a uma serie de dispositivos IoT conectados a plataforma Helix, plataforma essa que será responsavel 
por capturar os dados retornados dos dispositivos e salva-los em sua base de dados Mongodb, enquanto nossa aplicação consumirá 
e exibirá esses Dados em tempo real para os usuários.

Além de tudo, haverá também um Painel Administrador, da qual será incluido todas as funcionalidades relacionadas a configuração
dos dispositivos conectados, bem como seus parametros de notificação, e também para controle de usuários.

## Apresentação do Sistema

Video de Apresentação e Funcionamento do Sistema no Youtube:
[Video de apresentação](https://www.youtube.com/watch?v=FGh7rhUspQI)

# Primeiros passos
Para começar o desenvolvimento, será necessário seguir os seguintes passos:


### 1.	Configurações Iniciais
Configurações iniciais necessárias para colocar o projeto pra rodar.

- Primeiro de tudo acesse a pasta **BancoDeDados** e execute o script SQL do arquivo "scripts-db", para que toda o Banco de Dados
e tabelas sejam devidamente criados;
- Abra a pasta MVCAvisaEnchenteProject, e crie um arquivo chamado **"appsettings.Delevopment.json"**, e após isso, copie e cole 
tudo o que estiver no arquivo **"appsettings.json"**, e configure a DefaultConnection para a sua connection string local;  

# Rodar o Projeto

Após Realizado os primeiros passos, basta dar um Build na Solução, ou apertar F5 para roda-la


## Observações

É possivel que nesse momento, a licensa do Geocoder, do Google, possa ter expirado, e por isso o Mapa do Sistema pode não carregar ao executa-lo.
Nesse Caso você poderá utilizar sua propria Chave de Licensa do Geocode.

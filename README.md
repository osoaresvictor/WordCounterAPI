
# WordSearcher

## Introdução
![WordCounterAPI Architecture](https://raw.githubusercontent.com/osoaresvictor/WordCounterAPI/master/wordsearcher_architecture.png)
Trata-se do projeto de um Web App, um contador de um banco de  palavras, com foco em escalabilidade. De forma geral, o usuário pode fazer o upload de arquivos `csv` contendo apenas palavras, através da página `localhost:80/upload` e após isso, a aplicação irá processar os arquivos e armazenar em banco para posteriores consultas em `localhost:80/counter/< palavra >`.

## Fluxo de Execução

1. Usuário popula a API por meio da página de upload (`localhost:80/upload`), enviando arquivos CSV contendo apenas palavras;
2. A API salva esses arquivos em um volume compartilhado do Docker e enviar uma mensagem correspondente a cada arquivo na Fila do RabbitMQ, contendo a informação do caminho em disco;
3.  Os executáveis ficam escutando a fila continuamente e quando surge uma nova mensagem, algum deles retira da fila e processa o arquivo informado para o banco de dados;
4. Com o banco populado, basta acessar  `localhost:80/counter/PALAVRA_PESQUISAR`, onde *PALAVRA_PESQUISAR* corresponder ao termo a ser pesquisado no banco.

## Observações Sobre a Arquitetura

A aplicação foi desenvolvida utilizando:
- Docker
- ASP.NET Core 2.0 para a API;
- C# - NET CORE 2.0 para o Console APP;
- Servidor de Proxy NGINX para o balanceamento de carga na API;
- Redis para o banco de dados Chave-Valor;
- RabbitMQ para o serviço de mensageria.

Sobre o BackEnd, foi optado pelo .NET CORE por ser um framework Open Source e de fácil implementação, amplo suporte/comunidade, além de possuir uma performance relativamente boa. 

O NGINX foi implantado pensando num cenário de grandes acessos tanto para upload quanto para consultas, assim sendo, é possível distribuir a carga das requisições que chegam ao servidor.

O RabbitMQ foi uma ótima solução para auxiliar no desacoplamento das funções que seriam atribuídas à Web API, de forma que, quando um ou mais arquivos são enviados, os mesmos são armazenados em um repositório de acesso comum à API e aos executáveis responsáveis por processar estes arquivos. Esses executáveis possuem a função de ficar escutando a fila e assim que uma nova mensagem (no caso, o caminho do arquivo enviado), chega, os mesmos a retiram da fila e processam cada palavra do arquivo, armazenando o resultado num banco chave-valor. Uma vantagem interessante do RabbitMQ é que o mesmo já realiza o controle de concorrência das requisições em fila.

Dentro desse cenário, uma das melhores opções disponíveis foi o Redis, por questões de desempenho, visto que os dados são armazenados em cache e também pelo contexto, já que sabemos exatamente as chaves e os valores são apenas decimais para contagem de palavras nos arquivos enviados. Além disso, o Redis possui boa escalabilidade.

Toda a solução roda sob containers do Docker. Além de ser prático, é bastante escalável.

## Escalabilidade

Como supracitado, a arquitetura desse projeto foi feita pensando em grandes cenários, portanto, é possível tanto escalar o número de containers da Web API quanto dos executáveis que processam os arquivos e escutam a fila.

- Para modificar o número de containers da WEB API:
1. Modifique o arquivo `docker-compose.yml` para adicionar ou remover mais serviços, conforme consta no modelo do arquivo na pasta da solução *`./WordSearcher`*;
2. Modifique o arquivo `nginx.conf` para registrar as novas rotas desejadas (arquivo na pasta *`./WordSearcher/nginx`*)

- Para modificar o número de executáveis que processam os arquivos, basta executar no momento do deploy `docker-compose up --scale fileCounterStore=n`, onde *n* é o número de containers do aplicativo que você deseja executar simultaneamente.

## Build e Deploy

Dado que a solução utiliza Docker, é necessário que você tenha o docker instalado na sua máquina ([Versões Disponíveis e Requisitos](https://store.docker.com/search?offering=community&q=&type=edition)). Também é possível utilizar as plataformas EC2 da Amazon e o Azure da Microsoft.

Para instalar na máquina local, já com o Docker instalado, basta ir na pasta onde está o arquivo docker-compose.yml:
1. `docker-compose build`
2.  `docker-compose up`

E a aplicação já estará executando conforme os parâmetros definidos nos arquivos de configuração `nginx.conf` e `docker-compose.yml`.

Atualmente, o Docker possui um suporte razoável em ferramentas de CI/CD, como exemplo o VSTS/TFS, que já conta com plugins e modelos de pipelines já prontos para serem adaptados.

A plataforma EC2 da Amazon também oferece um ótimo serviço, contendo opções de escalabilidade dinâmica dentre outros recursos bastante interessantes.

# ATIVIDADE

**Tema escolhido:** Gestão de Resíduos - Notificação aos moradores sobre dias de coleta e separação adequada de resíduos.

"Agora é a hora de dar outra vida aos seus projetos usando uma abordagem sofisticada e em linha com os temas de cidades inteligentes que você selecionou antes.
Você vai aproveitar o poder do .NET Core 8 para criar serviços RESTful que atendam às necessidades do tema de cidade inteligente que escolher.

- Pedimos que você e seu grupo mergulhem profundamente no tema escolhido, indo além do básico para criar algo não apenas viável, mas robusto e pronto para produção;
- Desenvolver uma série de, no mínimo, quatro serviços RESTful robustos que não só atendam às necessidades básicas do projeto, mas que também ofereçam funcionalidades avançadas e integrações complexas;
- Como requisitos obrigatórios, cada controller desenvolvido deve possuir pelo menos um teste unitário para validar o status code 200 usando xUnit, demonstrando a eficácia e a robustez de seu código;
- A arquitetura de sua aplicação deve seguir estritamente o padrão MVVM, garantindo uma clara separação entre a lógica de apresentação e a lógica de negócios;
- Para os endpoints que listam dados, é essencial implementar um mecanismo de paginação, assegurando que a aplicação possa escalar e manejar grandes volumes de dados de forma eficiente;
- Para endpoints críticos, deve-se implementar mecanismos robustos de autenticação e autorização, garantindo que apenas usuários autorizados tenham acesso a funcionalidades sensíveis;
- Com seu conhecimento aprofundado em .NET Core 8, é crucial que você vá além dos simples endpoints;
- Esperamos que sejam implementadas as configurações iniciais, técnicas avançadas de validação e tratamento de exceções, além de aplicar rigorosos requisitos de segurança nos endpoints pertinentes;
- É vital que sua solução seja integrada a um banco de dados e que seja utilizado o conceito de migrações para assegurar uma gestão eficiente e escalável do esquema de banco de dados ao longo do tempo;
- Você também deve considerar a aplicação de outras práticas avançadas presentes no conteúdo do curso, como a otimização de consultas (paginação) melhorar o desempenho."

# TECNOLOGIAS UTILIZADAS

- .NET 8.0;
- Projeto ASP.NET Core Web API;
- EntityFramework ORM 8.0;
- Swagger;
- Nuget;
- Docker;
- xUnit.

# BOAS PRÁTICAS E DESIGN PATTERNS UTILIZADOS

- Model View ViewModel - MVVM;
- Services;
- Repositories;
- MVC - Model, View e Controller;
- Exceptions customizadas de acordo com as regras de negócio;
- Versionamento do banco de dados com Migrations;
- Injeção de dependência;
- Implementação de testes unitários com xUnit.

# MODELO DE ENTIDADE E RELACIONAMENTO

![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/c8dfeffb-d8f0-4dd4-bbbf-ea9f44b4a158)

# SEGURANÇA

- A segurança com autenticação Stateless foi implementada em todo o
  projeto. Por padrão, a regra para todos os endpoints (exceto o de usuário)
  é que requisições: GET – Roles USER, OPERADOR e ADMIN;
  POST/PUT/DELETE – Roles OPERADOR e ADMIN;
- Para o endpoint de usuário, a criação (POST) e o Login (POST) não
  necessitam de autenticação/autorização, contudo, a atualização de um
  usuário (PUT), visualização (GET) ou exclusão (DELETE) necessita de
  um token JWT com a Role ADMIN;
- O tempo de expiração do Token JWT padrão é de 5 minutos;
- Anatomia de um Token JWT de exemplo;
  
  ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/a6ae8f4c-8bce-4086-a4e3-fb42b6f613b0)
- Dados sensíveis foram inseridos no arquivo “appsettings.json”, como:
  strings de conexão com o banco de dados, usuário e senha SMTP...

  ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/329b51ac-d143-4c93-9308-7924deed159a)

# SWAGGER

- Toda a API está devidamente documentada no Swagger. A UI dele Inclui:
  - Versionamento;
    
    ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/8ad2b1d5-e6ef-4b8d-aed2-104b4cda7a4b)
  - Botão de autenticação “Token JWT”;
    
    ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/b7680863-6178-4b22-8952-3436cbc6cc48)
  - Comentários personalizados por endpoint.
    
    ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/adbe7981-e68f-4b5f-ae42-9bf3c706a5c7)

# INTEGRAÇÃO - SERVIDOR SMTP

- Para de fato atender a ideia de “notificação aos moradores” do tema
  escolhido, o endpoint “disparar notificações” faz uso de um servidor de
  SMTP gratuito chamado MailTrap. Ao acionar o endpoint passando o id
  de uma notificação (gerada automaticamente após a criação da agenda),
  um email será enviado para cada registro da tabela T_GR_MORADOR,
  em seguida o registro da notificação é excluído.
  Como boa prática, o método de envio de e-mail é Async.
  ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/cb163289-1e67-4286-8b0b-a6ab8b080350)

# TESTES UNITÁRIOS

- O conceito de DI foi amplamente utilizado no projeto, sendo o contêiner
  de DI “IServiceCollection” configurado no arquivo “program.cs”;
  
  ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/fb18a32c-0f6c-4f72-8acd-46766dd82a27)
- Utilizando o “xUnit”, e a biblioteca “MOQ” para criar um DbContext
  mockado, quatro testes unitários foram implementados, por controller,
  para validar os seguintes critérios em Actions GET:
  - Status Code 200;
  - Status Code 404 quando uma referência inválida for passada;
  - Status Code 200 quanndo uma referência válida for passada;
  - Três registros retornados.
    
    ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/9706962d-c1fa-4119-865b-5363e223bdbd)
    ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/3404c5a9-5583-40ca-b02c-bd519d544f4c)
- Execução de todos os testes a partir do “Text Explorer”.
  
  ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/c75d7825-c419-4afd-87d9-b275cc9cdb3e)

# OUTROS DETALHES DO PROJETO

- Todos os controllers possuem um método GET datado para a versão
  “v2” da API, com a funcionalidade de paginação por referência;
  ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/2525c4a9-359a-4367-90c1-7bc34c4533bd)
- O projeto segue estritamente o padrão MVVM, validando os atributos
  utilizando “DataAnnotations” – prontas ou customizadas;
  ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/9b5c1a03-be84-462a-8857-cac99fc7e358)
  ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/5e7c0d59-05df-47d1-9202-0ce8e1f0ea80)
- Tratamento de exceção também foi implementado, inclusive, exceptions
  personalizadas;
  ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/fac37437-504c-4a41-a10f-1116677f59fc)
- O conceito de “Migrations” foi implementado, utilizando o “Fluent API”
  na classe “DatabaseContext” e os “DbSets”;![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/cce336ca-1df3-4b1d-93c6-471c8bf50a59)
- Em alguns atributos específicos foi praticado o uso de Enums, como por
  exemplo no atributo Role da entidade Usuário;
  
  ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/b7762df8-b6c2-4450-b39d-d3c84a0488bd)
- Endpoints de POST e PUT devolvem no Body da requisição de resposta
  o objeto criado/atualizado, além de um atributo location no header da
  requisição de resposta com a URI de consulta correspondente.
  ![image](https://github.com/pedroferrarezzo/Gere-Residuo-Csharp-.NET-ASPNET-Atividade-Fiap/assets/124400471/479bf860-3552-438c-bfab-ff22d66dc5fd)

# CI/CD

### Arquitetura da Pipeline
![arquitetura](https://github.com/user-attachments/assets/4fe428d6-065a-40a4-a605-c74213a7c715)

### Criação da infraestrutura de deploy

- Criar uma máquina virtual no Azure:
  - Ubuntu Linux 24.04;
  - 2vcpu, 8GB RAM;
  - Autenticação por senha;
  - IP público;
  - Liberar no grupo de segurança de rede as portas escolhidas para os containers da API (TCP 80, 81, 82).
- Gerar um par de chaves SSH assimétricas, sem secret key `ssh-keygen -t rsa -b 4096`:
  - Copiar o conteúdo da chave pública `id_rsa.pub`;
  - Copiar o conteúdo do chave privada `id_rsa`
- Acessar a máquina virtual via SSH:
  - Instalar o docker -> https://docs.docker.com/engine/install/ubuntu/#installation-methods;
  - Acrescentar o usuário ao grupo do Docker, para que seja possível inicializar containers sem ser usuário root `sudo usermod -aG docker meuusuario`;
  - Encerrar a sessão ssh e acessar novamente;
  - Criar a estrutura de pastas no diretório raiz do servidor, concedendo as devidas permissões para o usuário da máquina:
    ```
    drwxr-xr-x 3 rm552309 rm552309 4096 Sep 24 13:06 development
    drwxr-xr-x 3 rm552309 rm552309 4096 Sep 23 23:37 production
    drwxr-xr-x 3 rm552309 rm552309 4096 Sep 23 23:23 staging
    ```
  - Acessar o arquivo `~/.ssh/authorized_keys` com o usuário criado e colar o conteúdo da chave pública `id_rsa.pub` gerada anteriormente.

### Popular as variáveis do GitHub Secrets

- DB_CONNECTION_DEVELOPMENT: String de conexão do banco de dados do ambiente de desenvolvimento;
  ```
  Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = ORACLE-DB-DEVELOPMENT)(PORT = 1521)))(CONNECT_DATA = (SID = xe)));Persist Security Info=True;User ID=development_user;Password=171204;Pooling=True;Connection Timeout=60;
  ```
- DB_CONNECTION_PRODUCTION: String de conexão do banco de dados do ambiente de produção;
  ```
  Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = ORACLE-DB-PRODUCTION)(PORT = 1521)))(CONNECT_DATA = (SID = xe)));Persist Security Info=True;User ID=production_user;Password=171204;Pooling=True;Connection Timeout=60;
  ```
- DB_CONNECTION_STAGING: String de conexão do banco de dados do ambiente de staging;
  ```
  Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = ORACLE-DB-STAGING)(PORT = 1521)))(CONNECT_DATA = (SID = xe)));Persist Security Info=True;User ID=staging_user;Password=171204;Pooling=True;Connection Timeout=60;
  ```
- DOCKERHUB_TOKEN: Personal Access Token gerado no Dockerhub;
- DOCKERHUB_USERNAME: Usuário do Dockerhub;
- MAILTRAP_HOST: Endereço do servidor SMTP MailTrap;
  ```
  Criar conta para obter o endereço -> https://mailtrap.io/pt/
  ```
- MAILTRAP_PASSWORD: Senha do usuário para autenticação no servidor SMTP do MailTrap;
  ```
  Criar conta para obter a senha -> https://mailtrap.io/pt/
  ```
- MAILTRAP_PORT: Porta do servidor SMTP do MailTrap;
  ```
  Criar conta para obter a porta -> https://mailtrap.io/pt/
  ```
- MAILTRAP_SENDER: Remetente utilizado para envio dos emails;
  ```
  dev@gereresiduo.com.br
  ```
- MAILTRAP_USER: Usuário utilizado para autenticar no servidor SMTP do MailTrap;
  ```
  Criar conta para obter o usuário -> https://mailtrap.io/pt/
  ```
- ORACLE_PASSWORD: Senha do usuário SYS/SYSTEM banco Oracle XE;
- PORT_INGRESS_CONTAINER_API_DEVELOPMENT: Porta que o container do ambiente de desenvolvimento expõe a API;
  ```
  8080 (TCP)
  ```
- PORT_INGRESS_CONTAINER_API_PRODUCTION: Porta que o container do ambiente de produção expõe a API;
  ```
  8080 (TCP)
  ```
- PORT_INGRESS_CONTAINER_API_STAGING: Porta que o container do ambiente de staging expõe a API; 
  ```
  8080 (TCP)
  ```
- PORT_INGRESS_CONTAINER_DB_DEVELOPMENT: Porta que o container do ambiente de desenvolvimento expõe o banco de dados;
  ```
  1521 (TCP)
  ```
- PORT_INGRESS_CONTAINER_DB_PRODUCTION: Porta que o container do ambiente de produção expõe o banco de dados;
  ```
  1521 (TCP)
  ```
- PORT_INGRESS_CONTAINER_DB_STAGING: Porta que o container do ambiente de staging expõe o banco de dados;
  ```
  1521 (TCP)
  ```
- PORT_INGRESS_HOST_API_DEVELOPMENT: Porta que será mapeada do host para o container da API do ambiente de desenvolvimento;
- PORT_INGRESS_HOST_API_PRODUCTION: Porta que será mapeada do host para o container da API do ambiente de produção;
- PORT_INGRESS_HOST_API_STAGING: Porta que será mapeada do host para o container da API do ambiente de staging;
- PORT_INGRESS_HOST_DB_DEVELOPMENT: Porta que será mapeada do host para o container do banco de dados do ambiente de desenvolvimento;
- PORT_INGRESS_HOST_DB_PRODUCTION: Porta que será mapeada do host para o container do banco de dados do ambiente de produção;
- PORT_INGRESS_HOST_DB_STAGING: Porta que será mapeada do host para o container do banco de dados do ambiente de staging;
- SSH_HOST: Endereço de IP público da máquina virtual criada anteriormente;
- SSH_PRIVATE_KEY: Conteúdo da chave privada gerada anteriormente `id_rsa`;
- SSH_USER: Usuário criado na máquina virtual.

### Testando o fluxo de CI/CD
- Clonar o repositório localmente;
- Criar uma branch a partir da branch master `git checkout master | git checkout -b xxx/nome-da-sua-branch`;
- Desenvolver uma nova funcionalidade e escrever os testes unitários correspondentes com xUnit, no projeto de teste;
- Efetuar o push para o repositório remoto `git push origin xxx/nome-da-sua-branch`;
- Abrir um Pull Request da branch `xxx/nome-da-sua-branch` para a branch de desenvolvimento;
  - Acompanhar a execução do script de CI.
- Aprovar o PR para a branch de desenvolvimento;
  - Acompanhar a execução do script de CD.
- Validar o deployment no ambiente de desenvolvimento;
  - Acesse o servidor via SSH:
    - Verifique os recursos docker criados;
      - `docker ps`;
      - `docker volume ls`;
      - `docker network ls`.
    - Verifique os arquivos criados, incluindo o de variáveis de ambiente `.env`;
      - `ls -la /gere-residuo-app/development/`.
  - Verifique as imagens geradas no Dockerhub.
- Abrir um Pull Request da branch de desenvolvimento para a branch de staging;
- Aprovar o PR para a branch de staging;
  - Acompanhar a execução do script de CD.
- Validar o deployment no ambiente de staging, com os mesmos passos aplicados anteriormente para o ambiente de desenvolvimento;
- Abrir um Pull Request da branch de staging para a branch de produção (master);
- Aprovar o PR para a branch de produção (master);
  - Acompanhar a execução do script de CD.
- Validar o deployment no ambiente de produção, com os mesmos passos aplicados anteriormente para o ambiente de staging.

# INFORMAÇÕES FINAS

- O arquivo `docker-compose.yaml` pode ser encontrado na pasta `Docker`. Ele consome variáveis de ambiente presentes no arquivo `.env`;
- O arquivo `Dockerfile` pode ser encontrado na pasta do projeto da API `Br.Com.Fiap.Gere.Residuo/Dockerfile`;
- Os scripts de inicialização dos bancos de dados de cada ambiente podem ser encontrado nas pastas:
  - `Docker/SCRIPTS-SQL-DEVELOPMENT`;
  - `Docker/SCRIPTS-SQL-PRODUCTION`;
  - `Docker/SCRIPTS-SQL-STAGING`.
- A imagem utilizado para o container do banco de dados (Oracle XE) é `gvenzl/oracle-xe` -> https://hub.docker.com/r/gvenzl/oracle-xe
- A `collection.json` do Insomnia pode ser encontrada na pasta `INSOMNIA/gereresiduocollection.json`.

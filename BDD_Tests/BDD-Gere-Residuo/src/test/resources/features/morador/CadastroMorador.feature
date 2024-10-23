# language: pt

@MORADOR @FUNCIONAL @CREATE
Funcionalidade: Cadastro de novo MORADOR
  Como ADMIN ou OPERADOR do sistema
  Quero cadastrar um novo morador
  Para eventuais aberturas de agendas

  Contexto: Autenticação na API para obtenção do Token JWT e criação de bairro para o morador
    Dado que eu tenha os seguintes dados de usuário:
      | atributo       | valor                             |
      | usuarioEmail   | adminbaseteste@gereresiduo.com.br |
      | usuarioSenha   | Teste123@                         |
    Quando uma requisição POST for enviada para a rota "/api/v1/Usuario/Login" de Login
    Então o status code esperado é o 200
    E o JSON Schema de validação a ser usado é o "Login de usuário bem sucedido"
    Então a resposta da requisição deve estar em conformidade com o JSON Schema selecionado
    E o Token JWT seja recuperado da resposta da API
    Então o Token JWT retornado deve ser valido com a Secret Key
    Dado que eu tenha os seguintes dados de bairro:
      | atributo            | valor                          |
      | bairroNome          | Bairro das Flores              |
      | quantidadeLixeiras  | 1                              |
      | pesoMedioLixeirasKg | 120                            |
    Quando uma requisição POST for enviada para a rota "/api/v1/Bairro" de cadastro de bairro
    Então o status code que a API de cadastro de Bairro deve retornar é o 201
    E o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Bairro é o "Cadastro de bairro bem-sucedido"
    Então a resposta da requisição da API de cadastro de Bairro deve estar em conformidade com o JSON Schema selecionado

  @HOOK_CLEAN_MORADOR_AFTER_SCENARIO @HOOK_CLEAN_BAIRRO_AFTER_SCENARIO
  Cenário: Cadastro de morador bem-sucedido
    Dado que eu tenha os seguintes dados de morador:
      | atributo     | valor                         |
      | moradorNome  | Morador BDD                   |
      | moradorEmail | moradorbdd@gereresiduo.com.br |
    Quando eu recuperar o ID do bairro criado no contexto
    Então uma requisição POST for enviada para a rota "/api/v1/Morador" de cadastro de morador
    E o status code que a API de cadastro de Morador deve retornar é o 201
    Então o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Morador é o "Cadastro de morador bem-sucedido"
    E a resposta da requisição da API de cadastro de Morador deve estar em conformidade com o JSON Schema selecionado

  @HOOK_CLEAN_BAIRRO_AFTER_SCENARIO
  Cenário: Cadastro de morador mal-sucedido ao não passar um atributo obrigatório
    Dado que eu tenha os seguintes dados de morador:
      | atributo     | valor                         |
      | moradorNome  | Morador BDD                   |
      | moradorEmail | moradorbdd@gereresiduo.com.br |
    Quando uma requisição POST for enviada para a rota "/api/v1/Morador" de cadastro de morador
    Então o status code que a API de cadastro de Morador deve retornar é o 400
    E a API de cadastro de Morador deve retornar um objeto JSON contendo uma mensagem de erro: "O Id do bairro do morador deve ser um número positivo maior que 0."
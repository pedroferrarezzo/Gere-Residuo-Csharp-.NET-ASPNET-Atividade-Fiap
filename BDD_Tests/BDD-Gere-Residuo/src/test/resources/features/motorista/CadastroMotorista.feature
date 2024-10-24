# language: pt

@MOTORISTA @FUNCIONAL @CREATE
Funcionalidade: Cadastro de novo MOTORISTA
  Como ADMIN ou OPERADOR do sistema
  Quero cadastrar um novo motorista
  Para eventuais aberturas de agendas

  Contexto: Autenticação na API para obtenção do Token JWT
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

  @HOOK_CLEAN_MOTORISTA_AFTER_SCENARIO
  Cenário: Cadastro de motorista bem-sucedido
    Dado que eu tenha os seguintes dados de motorista:
      | atributo              | valor          |
      | motoristaNome         | Mariana Mario  |
      | motoristaCpf          | 558.128.717-44 |
      | motoristaNrCelular    | 013899113      |
      | motoristaNrCelularDdd | 23             |
      | motoristaNrCelularDdi | 63             |
    Quando uma requisição POST for enviada para a rota "/api/v1/Motorista" de cadastro de motorista
    Então o status code que a API de cadastro de Motorista deve retornar é o 201
    E o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Motorista é o "Cadastro de motorista bem-sucedido"
    Então a resposta da requisição da API de cadastro de Motorista deve estar em conformidade com o JSON Schema selecionado

  Cenário: Cadastro de motorista mal-sucedido ao não passar um atributo obrigatório
    Dado que eu tenha os seguintes dados de motorista:
      | atributo              | valor          |
      | motoristaNome         | Mariana Mario  |
      | motoristaNrCelular    | 013899113      |
      | motoristaNrCelularDdd | 23             |
      | motoristaNrCelularDdi | 63             |
    Quando uma requisição POST for enviada para a rota "/api/v1/Motorista" de cadastro de motorista
    Então o status code que a API de cadastro de Motorista deve retornar é o 400
    E a API de cadastro de Motorista deve retornar um objeto JSON contendo uma mensagem de erro: "O cpf do motorista é obrigatório!"

  Cenário: Cadastro de motorista mal-sucedido ao especificar um CPF no formato incorreto
    Dado que eu tenha os seguintes dados de motorista:
      | atributo              | valor           |
      | motoristaNome         | Mariana Mario   |
      | motoristaCpf          | 558.128.717-4-0 |
      | motoristaNrCelular    | 013899113       |
      | motoristaNrCelularDdd | 23              |
      | motoristaNrCelularDdi | 63              |
    Quando uma requisição POST for enviada para a rota "/api/v1/Motorista" de cadastro de motorista
    Então o status code que a API de cadastro de Motorista deve retornar é o 400
    E a API de cadastro de Motorista deve retornar um objeto JSON contendo uma mensagem de erro: "O cpf do motorista deve estar no formato correto! (123.456.789-09)"
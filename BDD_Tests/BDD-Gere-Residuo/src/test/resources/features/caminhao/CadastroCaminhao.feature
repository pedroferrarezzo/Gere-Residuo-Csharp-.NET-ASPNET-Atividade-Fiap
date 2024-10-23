# language: pt

@CAMINHAO @FUNCIONAL @CREATE
Funcionalidade: Cadastro de novo CAMINHAO
  Como ADMIN ou OPERADOR do sistema
  Quero cadastrar um novo caminhao
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

  @HOOK_CLEAN_CAMINHAO_AFTER_SCENARIO
  Cenário: Cadastro de caminhao bem-sucedido
    Dado que eu tenha os seguintes dados de caminhao:
      | atributo       | valor      |
      | caminhaoPlaca  | COR4C62    |
      | dataFabricacao | 2024-06-07 |
      | caminhaoMarca  | Volkswagen |
      | caminhaoModelo | Volvo X    |
    Quando uma requisição POST for enviada para a rota "/api/v1/Caminhao" de cadastro de caminhao
    Então o status code que a API de cadastro de Caminhao deve retornar é o 201
    E o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Caminhao é o "Cadastro de caminhao bem-sucedido"
    Então a resposta da requisição da API de cadastro de Caminhao deve estar em conformidade com o JSON Schema selecionado

  Cenário: Cadastro de caminhao mal-sucedido ao não passar um atributo obrigatório
    Dado que eu tenha os seguintes dados de caminhao:
      | atributo       | valor      |
      | dataFabricacao | 2024-06-07 |
      | caminhaoMarca  | Volkswagen |
      | caminhaoModelo | Volvo X    |
    Quando uma requisição POST for enviada para a rota "/api/v1/Caminhao" de cadastro de caminhao
    Então o status code que a API de cadastro de Caminhao deve retornar é o 400
    E a API de cadastro de Caminhao deve retornar um objeto JSON contendo uma mensagem de erro: "A placa do caminhão é obrigatória!"
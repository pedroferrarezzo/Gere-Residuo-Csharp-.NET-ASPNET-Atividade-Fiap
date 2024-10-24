# language: pt

@AGENDA @REGRESSAO @CREATE
Funcionalidade: Abertura de agenda
  Como ADMIN ou OPERADOR do sistema
  Quero abrir uma nova agenda
  Para que posteriormente os moradores sejam notificados da coleta de lixo

  Contexto: Autenticação na API para obtenção do Token JWT e criação de entidades relacionadas
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

    # MORADOR E BAIRRO
    Dado que eu tenha os seguintes dados de bairro:
      | atributo            | valor                          |
      | bairroNome          | Bairro das Flores              |
      | quantidadeLixeiras  | 1                              |
      | pesoMedioLixeirasKg | 120                            |
    Quando uma requisição POST for enviada para a rota "/api/v1/Bairro" de cadastro de bairro
    Então o status code que a API de cadastro de Bairro deve retornar é o 201
    E o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Bairro é o "Cadastro de bairro bem-sucedido"
    Então a resposta da requisição da API de cadastro de Bairro deve estar em conformidade com o JSON Schema selecionado
    Dado que eu tenha os seguintes dados de morador:
      | atributo     | valor                         |
      | moradorNome  | Morador BDD                   |
      | moradorEmail | moradorbdd@gereresiduo.com.br |
    Quando eu recuperar o ID do bairro criado no contexto
    Então uma requisição POST for enviada para a rota "/api/v1/Morador" de cadastro de morador
    E o status code que a API de cadastro de Morador deve retornar é o 201
    Então o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Morador é o "Cadastro de morador bem-sucedido"
    E a resposta da requisição da API de cadastro de Morador deve estar em conformidade com o JSON Schema selecionado

    # CAMINHAO
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

    # MOTORISTA
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

  @HOOK_CLEAN_NOTIFICACAO_AFTER_SCENARIO @HOOK_CLEAN_AGENDA_AFTER_SCENARIO @HOOK_CLEAN_BAIRRO_AFTER_SCENARIO @HOOK_CLEAN_MORADOR_AFTER_SCENARIO @HOOK_CLEAN_CAMINHAO_AFTER_SCENARIO @HOOK_CLEAN_MOTORISTA_AFTER_SCENARIO
  Cenário: Cadastro de agenda bem-sucedido
    Dado que eu tenha os seguintes dados de agenda:
      | atributo            | valor                          |
      | diaColetaDeLixo     | 2026-10-25                     |
      | tipoResiduo         | PLASTICO                       |
    Quando eu recuperar os IDs de caminhão, motorista e bairro criados
    Então uma requisição POST for enviada para a rota "/api/v1/Agenda" de cadastro de agenda
    E o status code que a API de cadastro de Agenda deve retornar é o 201
    Então o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Agenda é o "Cadastro de agenda bem-sucedido"
    E a resposta da requisição da API de cadastro de Agenda deve estar em conformidade com o JSON Schema selecionado
    Então uma requisição GET deve ser enviada para "/api/v1/Bairro" passando o ID do bairro da agenda como Path Parameter para obter o seu estado atual
    E o atributo bairroEstaDisponivel deve ser igual a "False"
    Então uma requisição GET deve ser enviada para "/api/v1/Caminhao" passando o ID do caminhao da agenda como Path Parameter para obter o seu estado atual
    E o atributo caminhaoEstaDisponivel deve ser igual a "False"
    Então uma requisição GET deve ser enviada para "/api/v1/Motorista" passando o ID do motorista da agenda como Path Parameter para obter o seu estado atual
    E o atributo motoristaEstaDisponivel deve ser igual a "False"

  @HOOK_CLEAN_BAIRRO_AFTER_SCENARIO @HOOK_CLEAN_MORADOR_AFTER_SCENARIO @HOOK_CLEAN_CAMINHAO_AFTER_SCENARIO @HOOK_CLEAN_MOTORISTA_AFTER_SCENARIO
  Cenário: Cadastro de agenda mal-sucedido ao fornecer uma dia de coleta de lixo anterior ao dia atual
    Dado que eu tenha os seguintes dados de agenda:
      | atributo            | valor                          |
      | diaColetaDeLixo     | 2023-10-25                     |
      | tipoResiduo         | PLASTICO                       |
    Quando eu recuperar os IDs de caminhão, motorista e bairro criados
    Então uma requisição POST for enviada para a rota "/api/v1/Agenda" de cadastro de agenda
    E o status code que a API de cadastro de Agenda deve retornar é o 400
    Então a API de cadastro de Agenda deve retornar um objeto JSON contendo uma mensagem de erro que começe com: "A data de agenda da coleta de lixo é menor que o dia de hoje"


  @HOOK_CLEAN_BAIRRO_AFTER_SCENARIO @HOOK_CLEAN_MORADOR_AFTER_SCENARIO @HOOK_CLEAN_CAMINHAO_AFTER_SCENARIO @HOOK_CLEAN_MOTORISTA_AFTER_SCENARIO
  Cenário: Cadastro de agenda mal-sucedido ao especificar um ID de bairro inválido
    Dado que eu tenha os seguintes dados de agenda:
      | atributo            | valor                          |
      | diaColetaDeLixo     | 2026-10-25                     |
      | tipoResiduo         | PLASTICO                       |
    Quando eu recuperar os IDs de caminhão e motorista, especificando um ID de bairro inválido
    Então uma requisição POST for enviada para a rota "/api/v1/Agenda" de cadastro de agenda
    E o status code que a API de cadastro de Agenda deve retornar é o 400
    Então a API de cadastro de Agenda deve retornar um objeto JSON contendo uma mensagem de erro: "O Id do bairro deve ser um número positivo maior que 0."
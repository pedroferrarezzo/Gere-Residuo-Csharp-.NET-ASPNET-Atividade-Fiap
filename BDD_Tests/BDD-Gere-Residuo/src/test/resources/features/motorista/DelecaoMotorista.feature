# language: pt

@MOTORISTA @FUNCIONAL @DELETE
Funcionalidade: Deleção de motorista
  Como ADMIN ou OPERADOR do sistema
  Quero excluir um motorista
  Para fins de inutilização do registro

  Contexto: Autenticação na API para obtenção do Token JWT e criação de motorista para deleção
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

  Cenário: Exclusão bem-sucedida de motorista pelo ID
    Dado que eu recupere o ID do motorista criado
    Quando uma requisição DELETE for enviada para a rota "/api/v1/Motorista" passando o ID do motorista como Path Parameter
    Então o status code que a API de cadastro de Motorista deve retornar é o 204

  @HOOK_CLEAN_MOTORISTA_AFTER_SCENARIO
  Cenário: Exclusão mal-sucedida de motorista pelo ID
    Dado que eu especifique um ID de motorista invalido
    Quando uma requisição DELETE for enviada para a rota "/api/v1/Motorista" passando o ID do motorista como Path Parameter
    Então o status code que a API de cadastro de Motorista deve retornar é o 404
    E a API de cadastro de Motorista deve retornar um objeto JSON contendo uma mensagem de erro: "O motorista de ID: 0 não existe!"
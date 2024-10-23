# language: pt

@CAMINHAO @FUNCIONAL @DELETE
Funcionalidade: Deleção de caminhao
  Como ADMIN ou OPERADOR do sistema
  Quero excluir um caminhao
  Para fins de inutilização do registro

  Contexto: Autenticação na API para obtenção do Token JWT e criação de caminhão para deleção
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

  Cenário: Exclusão bem-sucedida de caminhao pelo ID
    Dado que eu recupere o ID do caminhao criado
    Quando uma requisição DELETE for enviada para a rota "/api/v1/Caminhao" passando o ID do caminhao como Path Parameter
    Então o status code que a API de cadastro de Caminhao deve retornar é o 204

  @HOOK_CLEAN_CAMINHAO_AFTER_SCENARIO
  Cenário: Exclusão mal-sucedida de caminhao pelo ID
    Dado que eu especifique um ID de caminhao invalido
    Quando uma requisição DELETE for enviada para a rota "/api/v1/Caminhao" passando o ID do caminhao como Path Parameter
    Então o status code que a API de cadastro de Caminhao deve retornar é o 404
    E a API de cadastro de Caminhao deve retornar um objeto JSON contendo uma mensagem de erro: "O caminhão de ID: 0 não existe!"
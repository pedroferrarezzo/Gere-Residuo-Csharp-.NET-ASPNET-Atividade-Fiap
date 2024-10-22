# language: pt

@AGENDA @FUNCIONAL @CREATE
Funcionalidade: Cadastro de novo usuário OPERADOR
  Como OPERADOR ou ADMIN do sistema
  Quero cadastrar uma nova agenda
  Para que os moradores sejam notificados da coleta de lixo



  Cenário: Cadastro de bairro para abertura de agenda
    Dado que eu tenha os seguintes dados de bairro:
      | atributo            | valor                          |
      | BairroNome          | Bairro das Flores              |
      | quantidadeLixeiras  | 1                              |
      | pesoMedioLixeirasKg | 120                            |
    Quando uma requisição POST for enviada para a rota "/api/v1/Bairro" de cadastro de bairro
    Então o status code esperado na resposta da API é o 201
    E o JSON Schema de validação a ser usado na resposta da API é o "Cadastro de usuário bem-sucedido"
    Então a resposta da requisição deve estar em conformidade com o JSON Schema escolhido
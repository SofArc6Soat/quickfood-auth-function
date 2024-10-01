- [Função Lambda de Autenticação](#função-lambda-de-autenticação)
  - [Como Utilizar](#como-utilizar)
    - [Estrutura de Requisição (Exemplo)](#estrutura-de-requisição-exemplo)
  - [Deploy com SAM](#deploy-com-sam)
    - [Pré-requisitos](#pré-requisitos)
    - [Passos para Build e Deploy](#passos-para-build-e-deploy)
      - [Efetuar o build da Lambda:](#efetuar-o-build-da-lambda)
      - [Efetuar o deploy da Lambda:](#efetuar-o-deploy-da-lambda)
      - [Excluir o stack Lambda:](#excluir-o-stack-lambda)
      - [Notas Finais](#notas-finais)

---

# Função Lambda de Autenticação

Esta função Lambda tem como objetivo identificar um cliente (usando seu CPF e senha) ou um funcionário (usando seu email e senha) cadastrados no Amazon Cognito, gerando um token JWT válido após a autenticação bem-sucedida. Todas as requisições para a função são realizadas via API Gateway.

## Como Utilizar

Para testar e utilizar esta função, você pode usar a collection do Postman chamada "Lambda Autenticação.postman_collection.json", que está localizada no diretório "postman". Com essa collection, você poderá realizar as seguintes ações:

1. **Autenticar um cliente**: Informando o CPF e senha.
2. **Autenticar um funcionário**: Informando o email e senha.

### Estrutura de Requisição (Exemplo)

- **Cliente**:
```json
{
  "cpf": "000.000.000-00",
  "senha": "suaSenha"
}
  ```
- **Funcionário**:
```json
{
  "email": "exemplo@empresa.com",
  "senha": "suaSenha"
}
```
**Retorno Esperado**:
Após uma autenticação bem-sucedida, será retornado um token JWT que pode ser usado para autenticação em outros serviços da sua aplicação.

## Deploy com SAM
Para efetuar o deploy desta função Lambda, siga os passos abaixo:

### Pré-requisitos
Certifique-se de ter as seguintes ferramentas instaladas no seu ambiente:

* SAM CLI - [Install the SAM CLI](https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/serverless-sam-cli-install.html)
* .NET Core - [Install .NET Core](https://www.microsoft.com/net/download)
* Docker - [Install Docker community edition](https://hub.docker.com/search/?type=edition&offering=community)

### Passos para Build e Deploy

#### Efetuar o build da Lambda:

Execute o comando abaixo para construir o projeto Lambda usando o SAM CLI:

```bash
sam build
```
#### Efetuar o deploy da Lambda:

Execute o comando abaixo para fazer o deploy da função Lambda. O parâmetro --guided fornecerá um modo interativo para configurar as opções de deploy, como o nome do stack, região, etc.

```bash
sam deploy --guided
```

#### Excluir o stack Lambda:

Caso você deseje remover a função Lambda do seu ambiente, você pode usar o seguinte comando:

```bash
sam delete --stack-name auth-lambda
```

#### Notas Finais
Certifique-se de que as credenciais AWS estejam configuradas corretamente no seu ambiente antes de realizar o deploy.
É importante que no Identity and Access Management (IAM) a função auth-lambda-NetCodeWebAPIServerlessRole-* possua a política AmazonCognitoReadOnly.
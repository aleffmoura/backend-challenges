# Técnicas e Arquitetura

Este projeto foi desenvolvido utilizando uma estrutura simples baseada na abordagem "folder by feature" e em uma arquitetura em camadas, com o intuito de minimizar o uso desnecessário de complexidade arquitetural.

Foi adotada uma abordagem combinada de Orientação a Objetos e técnicas de programação funcional.
No entanto, foi evitado um maior enfoque na programação funcional devido à natureza verbosa do C# para esse tipo de abordagem.

Algumas técnicas aplicadas incluem: Funções de primeira classe, funções em primeira ordem, Funções Anônimas (Lambda), Currying, Partial Application e Recursão.

# Frameworks/Bibliotecas

Foram utilizadas as seguintes ferramentas:

- Newtonsoft.Json: Para manipulação dos inputs inseridos pelo usuário.
- FluentAssertions: Oferecendo uma abordagem mais fluente ("fluent-api") para verificação dos casos de teste.
- Moq: Utilizado apenas em um teste.
- NUnit: Escolhido para a execução dos testes.

# Instalação
##### Existe duas maneira, uma por dotnet e outra por docker, a vontade para escolher

## Execução Simples
Se o objetivo for executar o programa via terminal, é necessário apenas instalar o [dotnet 7 runtime](https://dotnet.microsoft.com/pt-br/download/dotnet/7.0).
Dessa forma, será possível rodar em qualquer sistema e realizar a entrada de input.

Após a instalação, vá até a pasta 'console-app' e execute o seguinte comando:
```bash
./Challenge.CLI < files/case1.txt
```
Outra possivel utilização em caso do binario não funcionar no MAC
```bash
dotnet Challenge.CLI.dll < files/case1.txt
```

Altere 'files/case1.txt' para o caso que deseja testar ou utilize seu próprio arquivo. No entanto, deixei todos os exemplos dentro da pasta 'files'.


### Testes e Compilação
Para executar testes e compilar o projeto, é necessário instalar o [dotnet 7 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/7.0) e escolher a arquitetura correspondente ao seu sistema.
Após a instalação, no terminal, vá até a pasta 'solution' e execute o comando a seguir para compilar o arquivo .sln:
```bash
dotnet build *.sln
```
Para executar os testes, utilize o comando:
```bash
dotnet test *.sln
```
Este comando executará todos os testes presentes no arquivo .sln.

## Utilização com Docker
Dentro do diretório 'solution', encontra-se um arquivo docker-compose. Execute o seguinte comando para criar as imagens necessárias:
```bash
docker-compose up -d
```
Para executar o programa utilizando um arquivo de entrada, utilize o comando abaixo:

Altere 'files/case7.txt' para o caso que deseja testar ou utilize seu próprio arquivo, lembresse de esta na pasta que tem os arquivos ou digite o caminho inteiro do mesmo.
```bash
docker run -i challenge_cli < files/case7.txt
```
Para rodar os casos de teste, use o comando:
```bash
docker run challenge_cli_tests 
```

## Notas adicionais
Devido às características da linguagem C#, a implementação da imutabilidade no estilo de programação orientada a objetos pode ser um pouco complicada.

Os testes foram realizados em ambientes Windows e Linux para garantir a compatibilidade.
Há capturas de tela na pasta 'prints de execução', tanto da execução no Windows quanto no Linux.


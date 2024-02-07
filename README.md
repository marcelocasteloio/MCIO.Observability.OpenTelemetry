# MCIO.Core

[EM BRANCO]

## :book: Documentos

| Status | Nome | PT-BR |
| :- | :- | :- |
| :black_circle: Em construção | Benchmarks | [BENCHMARKS-PT.md](docs/BENCHMARKS-PT.md) |
| :black_circle: Em construção | Decisões de design | [DESIGN-DECISIONS-PT.md](docs/DESIGN-DECISIONS-PT.md) |

## :package: Pacotes

| Nome | Versão | Link | Repository |
| :- | :- | :- | :- |
| MarceloCastelo.IO.Core |  ![Nuget](https://img.shields.io/nuget/v/MarceloCastelo.IO.Core) | [Nuget.org](https://www.nuget.org/packages/MarceloCastelo.IO.Core/) | Esse aqui :) |


## :label: Labels

| Categoria | Descrição | Labels (todos os ícones são clicáveis e levam as ferramentas externas) |
|-|-|-|
| Licença | MIT | [![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/) |
| Segurança | Vulnerabilidades | [![CodeQL](https://github.com/marcelocasteloio/MCIO.Core/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/marcelocasteloio/MCIO.Core/actions/workflows/github-code-scanning/codeql) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.Core&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.Core) [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.Core&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.Core) |
| Qualidade | Visão geral | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.Core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.Core) |
| Qualidade | Cobertura de testes | [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.Core&metric=coverage)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.Core) |
| Qualidade | Teste de mutação | [![Mutation Test](https://github.com/marcelocasteloio/MCIO.Core/actions/workflows/mutation-test.yml/badge.svg)](https://github.com/marcelocasteloio/MCIO.Core/actions/workflows/mutation-test.yml) |
| Qualidade | Manutenabilidade | [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.Core&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.Core) |
| Qualidade | Confiabilidade | [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.Core&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.Core) |
| Qualidade | Bugs | [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.Core&metric=bugs)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.Core) |
| Qualidade | Dívidas técnicas | [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.Core&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.Core) |
| Qualidade | Linhas duplicadas (%) | [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.Core&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.Core) |
| Qualidade | Melhorias de código | [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.Core&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.Core) |
| Pipeline | Compilação e testes | [![Build and Test](https://github.com/marcelocasteloio/MCIO.Core/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/marcelocasteloio/MCIO.Core/actions/workflows/build-and-test.yml) |
| Pipeline | Publicação | [![Publish](https://github.com/marcelocasteloio/MCIO.Core/actions/workflows/publish.yml/badge.svg)](https://github.com/marcelocasteloio/MCIO.Core/actions/workflows/publish.yml) |

## :page_facing_up: Introdução

[EM BRANCO]

## :book: Conteúdo
- [MCIO.Core](#mciocore)
  - [:book: Documentos](#book-documentos)
  - [:package: Pacotes](#package-pacotes)
  - [:label: Labels](#label-labels)
  - [:page\_facing\_up: Introdução](#page_facing_up-introdução)
  - [:book: Conteúdo](#book-conteúdo)
  - [:package: Dependências](#package-dependências)
  - [:computer: Tecnologias](#computer-tecnologias)
  - [:star: Funcionalidades-chave](#star-funcionalidades-chave)
  - [:star: Roadmap](#star-roadmap)
  - [:rocket: Executando localmente](#rocket-executando-localmente)
  - [:books: Utilização básica](#books-utilização-básica)
  - [:books: Exemplos](#books-exemplos)
  - [:people\_holding\_hands: Contribuindo](#people_holding_hands-contribuindo)
  - [:people\_holding\_hands: Autores](#people_holding_hands-autores)

## :package: Dependências

[voltar ao topo](#book-conteúdo)

- [.NET Standard 2.0](https://learn.microsoft.com/pt-br/dotnet/standard/net-standard?tabs=net-standard-2-0).

## :computer: Tecnologias

[voltar ao topo](#book-conteúdo)

Esse projeto utiliza as seguintes tecnologias:
- `C#` como linguagem de programação.
- `.NET Standard 2.0` para o pacote nuget.
- `.NET 8` para os projetos de teste de unidade, benchmark e exemplos.
- `xUnit` como framework de testes de unidade.
- `FluentAssertions` para escrita dos Asserts dos testes de unidade de forma fluída.
- `SonarQube` para ferramenta de análise estática de código (SAST - *Static Application Security Testing*).
- `Stryker.NET` como framework para testes de mutação.
- `BenchmarkDotNet` como framework para realização dos benchmarks.
- `Github Actions` para as pipelines.
- `Github CodeQL` para análise de vulnerabilidades de segurança.
- `Nuget.org` como repositório de pacotes.

## :star: Funcionalidades-chave

[voltar ao topo](#book-conteúdo)

[EM BRANCO]

## :star: Roadmap

[voltar ao topo](#book-conteúdo)

[EM BRANCO]

## :rocket: Executando localmente

[voltar ao topo](#book-conteúdo)

Por se tratar de um pacote nuget, não existe uma execução. Porém, existe o script [build-local-script](build-local-script.ps1) que pode ser executado via PowerShell que realizará as seguintes ações:

1. Instalará a CLI do ReportGenerator localmente para visualização do relatório de cobertura no formato opencover.
2. Instalará a CLI do Stryker localmente para execução e visualização do relatório do teste de mutação.
3. Restore do projeto.
4. Build do projeto em modo release.
5. Execução dos testes de unidade.
6. Execução do teste de mutação.
7. Abertura do relatório de cobertura no navegador web padrão.
8. Abertura do relatório de teste mutante no navegador web padrão.

A partir do `diretóio raiz` do repositório, no `PowerShell`, execute o comando `.\build-local-script.ps1`.

Caso queira limpar todos os arquivos gerados, a partir do `diretóio raiz` do repositório, no `PowerShell`, execute o comando `.\clear-local-script.ps1`.

## :books: Utilização básica

[voltar ao topo](#book-conteúdo)

[EM BRANCO]

## :books: Exemplos

[voltar ao topo](#book-conteúdo)

[EM BRANCO]

## :people_holding_hands: Contribuindo

[voltar ao topo](#book-conteúdo)

Você está mais que convidado para constribuir. Caso tenha interesse e queira participar do projeto, não deixe de ver nosso [manual de contribuição](docs/CONTRIBUTING-PT.md). 

## :people_holding_hands: Autores

[voltar ao topo](#book-conteúdo)

- Marcelo Castelo Branco - [@MarceloCas](https://www.linkedin.com/in/marcelocastelobranco/)

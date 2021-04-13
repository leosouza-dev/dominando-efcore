# Dominando o Entity Framework Core

## EF Database

### Para que serve c Deleted/Created

- Alternativa às migrações, usados normalmente em ambiente de DEV.
- Ensure Created - cria o BD e tabelas (se não existir).
- Ensure Deleted - exclui um BD.

### Resolvendo GAP do EnsureCreated para múltiplos contextos

- Para exemplificar foi criado uma classe Cidade e um contexto novo para essa classe, porém usando a mesma string de conexão;
- Na classe Program foi criado um método novo - GapDoEnsureCreated();
- Nesse método estamos tentando usar o EnsureCreated para cada contexto;
- Ao rodar a aplicação, só é criado as tabelas do primeiro contexto - esse é o GAP;
- Não é possível rodar o EnsureCreated para contextos diferentes que usam o mesmo BD;
- Resolvendo - Criamos o databaseCreator e usamos o método CreateTables();

### HealthCheck do banco de dados

- Validando a conexão;
- Vamos criar o método na classe Program - HealthCheckBancoDeDados();
- Foi mostrado duas formas bem comum até então - connection.Open() ou usando o DbSet;
- Agora temos um método novo para isso - CanConnect();

### Gerenciando o estado da Conexão

- Por padrão o EFCore abre e fecha a conexão com o BD sempre que é utilizado;
- Isso é uma boa prática, porém existem casos que é necessário, durante um processo realizar a ida a base de dados várias vezes dentro de um método, por exemplo;
- Vamos entender quando deixamos o EFCore cuidar do estado da sua conexão ou quando devemos nos preocupar em cuidar disso;
- Vamos criar um método - GerenciarEstadoDaConexao() que será nosso exemplo para testes;
- Esse método irá realizar 200 consultas no banco de dados e vamos contar quanto tempo dura esse processo. Vamos testar das duas formas;
- Rodas a aplicação com o EF cuidando da sua conexão - **tempo: 00:00:01.9636781**;
- Porém instrutor falou que código ainda não é o suficiente para demonstrar o que eles quer;
- Vamos criar um parâmetro no método GerenciarEstadoDaConexao() que quando for recebido algo, ele irá gerenciar;
- Quando recebermos "true" no parâmetro, vamos abrir manualmente a conexão com o BD;
- GerenciarEstadoDaConexao(false) - **Tempo: 00:00:01.3884937, False**
- GerenciarEstadoDaConexao(true) - **Tempo: 00:00:00.0731434, True**;
- outran coisa que podemos testar é verificar quantas conexões são abertas no processo - criando uma varável estática.
- Até aqui tudo bem, conseguimos melhorar a performance da nossa aplicação, porém qual o problema acontecerá se o BD não estiver com o Polling habilitado?
- Se o Polling estar ativado, o tempo de processamento, quando o EFCore gerencia a conexão (abrind 200x), sobe em mais de 2 segundos, enquanto quando nós estamos gerenciando continua igual.

### Conhecendo os comandos ExecuteSql

- Vamos criar um método ExecuteSQL() para conhecermos melhor e testar;
- Um dos métodos mais comuns é  o CreateCommand() em conjunto com cmd.CommandText  e cmd.ExecuteNonQuery(), porém não é o mais seguro;
- Uma forma mais segura é usando o ExecuteSqlRaw();
- Com essa segunda opção podemos trabalhar com DbParameters - evitando possíveis ataques;
- Também existe o método ExecuteSqlInterpolated(), que é parecido com o ExecuteSqlRaw, só que trabalhando com uma string interpolada;

### Como se proteger de ataques do tipo SQL Injection?

- Na aula passada vimos dois métodos que nos ajudam a se proteger - ExecuteSqlRaw e ExecuteSqlInterpolated;
- Temos que tomar muito cuidado ao inserir valores que podem ser passados pelo usuário dentro das instruções SQL;
- Esse tipo de ataque ainda é muito comum;
- O ideal é sempre enviar esses valores como parâmetros;
- Vamos criar o método SqlInjection() para criar um ambiente de teste desse assunto;
- Vamos testar como atualizar um dado de forma segura e na sequência uma forma insegura;
- SImulando SqlINjection - foi interpolado no ExecuteRaw o valor "fornecido" - NÃO FAZER ISSO!!!;

### Detectando migrações Pendentes

- Vamos conhecer um método de extensão do DataBase que verifica em tempo de execução migrações pendentes;
- O métode é: GePendingMigration();
- E para testar esse Extension Method vamos criar nosso método para teste: - MigracoesPendentes;

// Vitor Stüker de Almeida
// Luiz Eduardo Simon

namespace BibliotecaPOO
{
    // ---------------------------------------------------------
    // 1. Classe Livro
    //    - Representa a menor unidade de informação do sistema.
    // ---------------------------------------------------------
    public class Livro
    {
        // Propriedades imutáveis definidas no construtor: título, autor e ISBN.
        public string Titulo { get; }
        public string Autor { get; }
        public string ISBN { get; }

        // Já que só o próprio livro controla seu estado, o set é privado.
        public bool Disponivel { get; private set; }

        // Ao instanciar um livro, presume-se que ele esteja disponível.
        public Livro(string titulo, string autor, string isbn)
        {
            Titulo = titulo;
            Autor = autor;
            ISBN = isbn;
            Disponivel = true;
        }

        // Tenta marcar o livro como emprestado. Retorna se deu certo.
        public bool Emprestar()
        {
            if (!Disponivel) return false; // Protege a integridade do acervo.
            Disponivel = false;
            return true;
        }

        // Marca o livro como devolvido.
        public void Devolver() => Disponivel = true;

        // Exibe informações resumidas do livro.
        public void ExibirInformacoes() =>
            Console.WriteLine($"Título: {Titulo} | Autor: {Autor} | ISBN: {ISBN} | Disponível: {Disponivel}");
    }

    // ---------------------------------------------------------
    // 2. Classe abstrata Usuario
    //    - Fator comum entre Aluno e Professor (herança).
    // ---------------------------------------------------------
    public abstract class Usuario
    {
        public string Nome { get; }
        public int Id { get; }

        protected Usuario(string nome, int id)
        {
            Nome = nome;
            Id = id;
        }

        // Método polimórfico: cada subclasse diz “quem ela é”.
        public abstract string ExibirTipoUsuario();

        // Override para facilitar impressão de objetos.
        public override string ToString() => $"{Nome} ({ExibirTipoUsuario()})";
    }

    // ---------------------------------------------------------
    // 3. Aluno: especializa Usuario com o atributo Curso.
    // ---------------------------------------------------------
    public class Aluno : Usuario
    {
        public string Curso { get; }

        public Aluno(string nome, int id, string curso) : base(nome, id) =>
            Curso = curso;

        public override string ExibirTipoUsuario() => "Aluno";
    }

    // ---------------------------------------------------------
    // 4. Professor: especializa Usuario com o atributo Departamento.
    // ---------------------------------------------------------
    public class Professor : Usuario
    {
        public string Departamento { get; }

        public Professor(string nome, int id, string departamento) : base(nome, id) =>
            Departamento = departamento;

        public override string ExibirTipoUsuario() => "Professor";
    }

    // ---------------------------------------------------------
    // 5. Classe Emprestimo
    //    - Demonstra *composição*: depende de Livro e Usuario.
    // ---------------------------------------------------------
    public class Emprestimo
    {
        public Livro LivroEmprestado { get; }
        public Usuario UsuarioRequisitante { get; }
        public DateTime DataEmprestimo { get; }
        public DateTime? DataDevolucao { get; private set; } // Nullable: pode ainda não ter devolução.

        public Emprestimo(Livro livro, Usuario usuario)
        {
            LivroEmprestado = livro;
            UsuarioRequisitante = usuario;
            DataEmprestimo = DateTime.Now;
        }

        // Registra devolução e libera o livro no acervo.
        public void RegistrarDevolucao()
        {
            DataDevolucao = DateTime.Now;
            LivroEmprestado.Devolver();
        }

        // Apresenta um resumo amigável do empréstimo.
        public void ExibirResumoEmprestimo()
        {
            Console.WriteLine("Resumo do Empréstimo:");
            Console.WriteLine($"Livro: {LivroEmprestado.Titulo}");
            Console.WriteLine($"Usuário: {UsuarioRequisitante}");
            Console.WriteLine($"Data do Empréstimo: {DataEmprestimo:dd/MM/yyyy}");
            if (DataDevolucao.HasValue)
                Console.WriteLine($"Data da Devolução: {DataDevolucao.Value:dd/MM/yyyy}");
        }
    }

    // ---------------------------------------------------------
    // 6. Programa de Teste (Main)
    //    - Demonstra os requisitos solicitados pela bibliotecária.
    // ---------------------------------------------------------
    internal static class Program
    {
        private static void Main()
        {
            // --- Cadastro de livros -----------------------------
            var livro1 = new Livro("Introdução à POO", "José da Silva", "978-85-333-1234-5");
            var livro2 = new Livro("Estruturas de Dados", "Maria Souza", "978-85-333-5678-9");

            // --- Cadastro de usuários ---------------------------
            var aluno = new Aluno("João Silva", 1, "Engenharia de Software");
            var professor = new Professor("Dra. Ana Costa", 2, "Computação");

            // --- Tentativa de empréstimo ------------------------
            if (livro1.Emprestar())
            {
                var emprestimo = new Emprestimo(livro1, aluno);
                Console.WriteLine("Livro emprestado com sucesso!\n");
                emprestimo.ExibirResumoEmprestimo();

                // Mostra que o livro ficou indisponível após o empréstimo.
                Console.WriteLine("\n-- Estado do livro depois do empréstimo --");
                livro1.ExibirInformacoes();

                // --- Devolução -----------------------------------
                Console.WriteLine("\nProcessando devolução...");
                emprestimo.RegistrarDevolucao();
                Console.WriteLine("Livro devolvido com sucesso!");

                Console.WriteLine("\n-- Estado do livro depois da devolução --");
                livro1.ExibirInformacoes();
            }
            else
            {
                Console.WriteLine("Não foi possível emprestar: livro já está emprestado.");
            }

            // --- Exibir tipos de usuário (polimorfismo) ---------
            Console.WriteLine("\n-- Tipos de Usuário no Sistema --");
            Console.WriteLine(aluno.ExibirTipoUsuario());
            Console.WriteLine(professor.ExibirTipoUsuario());
        }
    }
}

/* Saída do programa:

Livro emprestado com sucesso!

Resumo do Empréstimo:
Livro: Introdução à POO
Usuário: João Silva (Aluno)
Data do Empréstimo: 13/06/2025

-- Estado do livro depois do empréstimo --
Título: Introdução à POO | Autor: José da Silva | ISBN: 978-85-333-1234-5 | Disponível: False

Processando devolução...
Livro devolvido com sucesso!

-- Estado do livro depois da devolução --
Título: Introdução à POO | Autor: José da Silva | ISBN: 978-85-333-1234-5 | Disponível: True

-- Tipos de Usuário no Sistema --
Aluno
Professor

*/

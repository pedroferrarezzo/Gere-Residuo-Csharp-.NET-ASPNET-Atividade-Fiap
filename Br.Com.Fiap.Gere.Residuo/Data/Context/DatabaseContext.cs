using Br.Com.Fiap.Gere.Residuo.Model;
using Microsoft.EntityFrameworkCore;

namespace Br.Com.Fiap.Gere.Residuo.Data.Context
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<UsuarioModel> TabelaUsuario {  get; set; }
        public virtual DbSet<NotificacaoModel> TabelaNotificacao { get; set; }
        public virtual DbSet<AgendaModel> TabelaAgenda { get; set; }
        public virtual DbSet<BairroModel> TabelaBairro  { get; set; }
        public virtual DbSet<MoradorModel> TabelaMorador { get; set; }
        public virtual DbSet<MotoristaModel> TabelaMotorista { get; set; }
        public virtual DbSet<CaminhaoModel> TabelaCaminhao { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        protected DatabaseContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // T_GR_USUARIO
            modelBuilder.Entity<UsuarioModel>(entity =>
            {
                entity.ToTable("T_GR_USUARIO");
                entity.HasKey(u => u.UsuarioId);
                entity.Property(u => u.UsuarioNome)
                    .IsRequired()
                    .HasColumnName("usuario_nome");
                entity.Property(u => u.UsuarioEmail)
                    .IsRequired()
                    .HasColumnName("usuario_email");
                entity.Property(u => u.UsuarioSenha)
                    .IsRequired()
                    .HasColumnName("usuario_senha");
                entity.Property(u => u.UsuarioRole)
                     .HasConversion(
                        r => r.ToString(),
                        r => (UsuarioRole)Enum.Parse(typeof(UsuarioRole), r))
                    .IsRequired()
                    .HasColumnName("usuario_role");

                entity.HasIndex(u => u.UsuarioEmail).IsUnique();
            });

            // T_GR_NOTIFICACAO
            modelBuilder.Entity<NotificacaoModel>(entity =>
            {
                entity.ToTable("T_GR_NOTIFICACAO");
                entity.HasKey(n => n.NotificacaoId);
                entity.Property(n => n.NotificacaoId).HasColumnName("notificacao_id");
                entity.Property(n => n.AgendaId).HasColumnName("agenda_id");

                // Relacionamento de 1:1 com AgendaModel
                entity.HasOne(n => n.AgendaDaNotificacao)
                    .WithOne(a => a.NotificacaoGeradaParaEstaAgenda)
                    // Define a chave estrangeira
                    .HasForeignKey<NotificacaoModel>(n => n.AgendaId)
                    // Torna a chave estrangeira obrigatória
                    .IsRequired()
                    .HasConstraintName("FK_T_GR_NOTIFICACAO_AGENDA")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // T_GR_AGENDA
            modelBuilder.Entity<AgendaModel>(entity =>
            {
                entity.ToTable("T_GR_AGENDA");
                entity.HasKey(a => a.AgendaId);
                entity.Property(a => a.AgendaId).HasColumnName("agenda_id");
                entity.Property(a => a.DiaColetaDeLixo)
                    .IsRequired()
                    .HasColumnName("dia_coleta_de_lixo");
                entity.Property(a => a.TipoResiduo)
                    .HasConversion(
                        t => t.ToString(),
                        t => (TipoResiduo)Enum.Parse(typeof(TipoResiduo), t))
                    .IsRequired()
                    .HasColumnName("tipo_residuo");
                entity.Property(a => a.StatusColetaDeLixoAgendada)
                     .HasConversion(
                        s => s.ToString(),
                        s => (StatusColetaDeLixo)Enum.Parse(typeof(StatusColetaDeLixo), s))
                    .IsRequired()
                    .HasColumnName("status_coleta_de_lixo_agendada");
                entity.Property(a => a.PesoColetadoDeLixoKg)
                    .IsRequired()
                    .HasColumnName("peso_coletado_de_lixo_kg");
                entity.Property(a => a.CaminhaoId).HasColumnName("caminhao_id");
                entity.Property(a => a.MotoristaId).HasColumnName("motorista_id");
                entity.Property(a => a.BairroId).HasColumnName("bairro_id");

                // Relacionamento de 1:N com CaminhaoModel
                entity.HasOne(a => a.CaminhaoAlocadoParaAgenda)
                    .WithMany(c => c.AgendasCriadasComEsteCaminhao)
                    // Define a chave estrangeira
                    .HasForeignKey(a => a.CaminhaoId)
                    // Torna a chave estrangeira obrigatória
                    .IsRequired()
                    .HasConstraintName("FK_T_GR_AGENDA_CAMINHAO")
                    .OnDelete(DeleteBehavior.Restrict);

                // Relacionamento de 1:N com MotoristaModel
                entity.HasOne(a => a.MotoristaAlocadoParaAgenda)
                    .WithMany(m => m.AgendasCriadasComEsteMotorista)
                    // Define a chave estrangeira
                    .HasForeignKey(a => a.MotoristaId)
                    // Torna a chave estrangeira obrigatória
                    .IsRequired()
                    .HasConstraintName("FK_T_GR_AGENDA_MOTORISTA")
                    .OnDelete(DeleteBehavior.Restrict);

                // Relacionamento de 1:N com BairroModel
                entity.HasOne(a => a.BairroAgendadoParaColeta)
                    .WithMany(b => b.AgendasDeColetaDeLixoDoBairro)
                    // Define a chave estrangeira
                    .HasForeignKey(a => a.BairroId)
                    // Torna a chave estrangeira obrigatória
                    .IsRequired()
                    .HasConstraintName("FK_T_GR_AGENDA_BAIRRO")
                    .OnDelete(DeleteBehavior.Restrict);

            });

            // T_GR_BAIRRO
            modelBuilder.Entity<BairroModel>(entity =>
            {
                entity.ToTable("T_GR_BAIRRO");
                entity.HasKey(b => b.BairroId);
                entity.Property(b => b.BairroId).HasColumnName("bairro_id");
                entity.Property(b => b.BairroNome)
                    .IsRequired()
                    .HasColumnName("bairro_nome");
                entity.Property(b => b.QuantidadeLixeiras)
                    .IsRequired()
                    .HasColumnName("quantidade_lixeiras");
                entity.Property(b => b.PesoMedioLixeirasKg)
                    .IsRequired()
                    .HasColumnName("peso_medio_lixeiras_kg");
                entity.Property(b => b.PercentualColetaLixoBairro)
                    .IsRequired()
                    .HasColumnName("percentual_coleta_lixo_bairro");
                entity.Property(b => b.BairroEstaDisponivel)
                    .IsRequired()
                    .HasColumnName("bairro_esta_disponivel")
                    .HasConversion<int>(); // Converte BOOLEAN para int

                entity.HasIndex(b => b.BairroNome).IsUnique();
            });

            // T_GR_MORADOR
            modelBuilder.Entity<MoradorModel>(entity =>
            {
                entity.ToTable("T_GR_MORADOR");
                entity.HasKey(m => m.MoradorId);
                entity.Property(m => m.MoradorId).HasColumnName("morador_id");
                entity.Property(m => m.MoradorNome)
                    .IsRequired()
                    .HasColumnName("morador_nome");
                entity.Property(m => m.MoradorEmail)
                    .IsRequired()
                    .HasColumnName("morador_email");
                entity.Property(m => m.BairroId).HasColumnName("bairro_id");

                entity.HasIndex(m => m.MoradorEmail).IsUnique();

                // Relacionamento de 1:N com BairroModel
                entity.HasOne(m => m.BairroDoMorador)
                    .WithMany(b => b.MoradoresDoBairro)
                    // Define a chave estrangeira
                    .HasForeignKey(m => m.BairroId)
                    // Torna a chave estrangeira obrigatória
                    .IsRequired()
                    .HasConstraintName("FK_T_GR_MORADOR_BAIRRO")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // T_GR_MOTORISTA
            modelBuilder.Entity<MotoristaModel>(entity =>
            {
                entity.ToTable("T_GR_MOTORISTA");
                entity.HasKey(m => m.MotoristaId);
                entity.Property(m => m.MotoristaId).HasColumnName("motorista_id");
                entity.Property(m => m.MotoristaNome)
                    .IsRequired()
                    .HasColumnName("motorista_nome");
                entity.Property(m => m.MotoristaCpf)
                    .IsRequired()
                    .HasColumnName("motorista_cpf");
                entity.Property(m => m.MotoristaNrCelular)
                    .IsRequired()
                    .HasColumnName("motorista_nr_celular");
                entity.Property(m => m.MotoristaNrCelularDdd)
                    .IsRequired()
                    .HasColumnName("motorista_nr_celular_ddd");
                entity.Property(m => m.MotoristaNrCelularDdi)
                    .IsRequired()
                    .HasColumnName("motorista_nr_celular_ddi");
                entity.Property(m => m.MotoristaEstaDisponivel)
                    .IsRequired()
                    .HasColumnName("motorista_esta_disponivel")
                    .HasConversion<int>(); // Converte BOOLEAN para int

                entity.HasIndex(m => m.MotoristaNrCelular).IsUnique();
                entity.HasIndex(m => m.MotoristaCpf).IsUnique();
            });

            // T_GR_CAMINHAO
            modelBuilder.Entity<CaminhaoModel>(entity =>
            {
                entity.ToTable("T_GR_CAMINHAO");
                entity.HasKey(c => c.CaminhaoId);
                entity.Property(c => c.CaminhaoId).HasColumnName("caminhao_id");
                entity.Property(c => c.CaminhaoPlaca)
                    .IsRequired()
                    .HasColumnName("caminhao_placa");
                entity.Property(c => c.DataFabricacao)
                    .IsRequired()
                    .HasColumnName("data_fabricacao");
                entity.Property(c => c.CaminhaoMarca)
                    .IsRequired()
                    .HasColumnName("caminhao_marca");
                entity.Property(c => c.CaminhaoModelo)
                    .IsRequired()
                    .HasColumnName("caminhao_modelo");
                entity.Property(c => c.CaminhaoEstaDisponivel)
                    .IsRequired()
                    .HasColumnName("caminhao_esta_disponivel")
                    .HasConversion<int>(); // Converte BOOLEAN para int

                entity.HasIndex(c => c.CaminhaoPlaca).IsUnique();
            });
        }

    }
}

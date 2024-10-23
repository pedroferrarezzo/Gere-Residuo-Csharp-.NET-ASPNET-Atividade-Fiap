package hook.agenda;

import io.cucumber.java.After;
import lombok.Getter;
import lombok.Setter;
import model.notificacao.NotificacaoModel;
import services.agenda.CadastroAgendaService;
import services.notificacao.NotificacaoService;
import services.usuario.CadastroUsuarioService;

public class AgendaHook {
    @Getter
    @Setter
    private static String agendaCriadaId;
    private static final CadastroAgendaService cadastroAgendaService = new CadastroAgendaService();
    private static final CadastroUsuarioService cadastroUsuarioService = new CadastroUsuarioService();
    private static final NotificacaoService notificacaoService = new NotificacaoService();
    private static final String emailAdminBaseTeste = "adminbaseteste@gereresiduo.com.br";
    private static final String senhaAdminBaseTeste = "Teste123@";

    @After(value = "@HOOK_CLEAN_AGENDA_AFTER_SCENARIO", order = 3)
    public static void afterExcluirAgendaCriada() {
        String tokenJwt = cadastroUsuarioService.authenticateUsuario("/api/v1/Usuario/Login", emailAdminBaseTeste, senhaAdminBaseTeste);
        NotificacaoModel notificacaoDaAgenda = notificacaoService.getNotificacaoDaAgenda("/api/v1/Notificacao", tokenJwt, Integer.parseInt(agendaCriadaId));
        notificacaoService.deleteNotificacao("/api/v1/Notificacao", tokenJwt, String.valueOf(notificacaoDaAgenda.getNotificacaoId()));
        cadastroAgendaService.deleteAgenda("/api/v1/Agenda", tokenJwt, agendaCriadaId);
    }
}

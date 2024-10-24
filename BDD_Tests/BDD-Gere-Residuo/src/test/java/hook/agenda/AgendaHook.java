package hook.agenda;

import io.cucumber.java.After;
import lombok.Getter;
import lombok.Setter;
import model.notificacao.NotificacaoModel;
import services.agenda.CadastroAgendaService;
import services.notificacao.CadastroNotificacaoService;
import services.usuario.CadastroUsuarioService;

public class AgendaHook {
    @Getter
    @Setter
    private static String agendaCriadaId;
    private static final CadastroAgendaService cadastroAgendaService = new CadastroAgendaService();
    private static final CadastroUsuarioService cadastroUsuarioService = new CadastroUsuarioService();
    private static final String emailAdminBaseTeste = "adminbaseteste@gereresiduo.com.br";
    private static final String senhaAdminBaseTeste = "Teste123@";

    @After(value = "@HOOK_CLEAN_AGENDA_AFTER_SCENARIO", order = 2)
    public static void afterExcluirAgendaCriada() {
        String tokenJwt = cadastroUsuarioService.authenticateUsuario("/api/v1/Usuario/Login", emailAdminBaseTeste, senhaAdminBaseTeste);
        cadastroAgendaService.deleteAgenda("/api/v1/Agenda", tokenJwt, agendaCriadaId);
    }
}

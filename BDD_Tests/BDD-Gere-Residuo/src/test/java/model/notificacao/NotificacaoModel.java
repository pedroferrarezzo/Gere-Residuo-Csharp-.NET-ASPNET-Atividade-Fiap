package model.notificacao;

import com.google.gson.annotations.Expose;
import lombok.Data;
import model.agenda.AgendaModel;

@Data
public class NotificacaoModel {
    @Expose
    private int notificacaoId;
    @Expose
    private AgendaModel agendaDaNotificacao;
}

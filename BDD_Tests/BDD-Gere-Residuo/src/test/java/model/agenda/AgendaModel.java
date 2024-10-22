package model.agenda;

import com.google.gson.annotations.Expose;
import lombok.Data;
import model.enums.StatusColetaDeLixo;
import model.enums.TipoResiduo;

import java.util.Date;

@Data
public class AgendaModel {
    @Expose(serialize = false)
    private int agendaId;
    @Expose(serialize = false)
    private int pesoColetadoDeLixoKg;

    @Expose
    private int caminhaoId;
    @Expose
    private int motoristaId;
    @Expose
    private int bairroId;
    @Expose
    private String diaColetaDeLixo;
    @Expose
    private StatusColetaDeLixo statusColetaDeLixoAgendada;
    @Expose
    private TipoResiduo tipoResiduo;

}

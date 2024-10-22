package model.bairro;

import com.google.gson.annotations.Expose;
import lombok.Data;

@Data
public class BairroModel {

    @Expose(serialize = false)
    private int bairroId;
    @Expose(serialize = false)
    private int percentualColetaLixoBairro;
    @Expose(serialize = false)
    private boolean bairroEstaDisponivel;
    @Expose
    private String bairroNome;
    @Expose
    private int quantidadeLixeiras;
    @Expose
    private long pesoMedioLixeirasKg;
}

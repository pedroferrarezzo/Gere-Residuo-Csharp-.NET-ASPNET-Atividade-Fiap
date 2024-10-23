package model.caminhao;

import com.google.gson.annotations.Expose;
import lombok.Data;

import java.util.Date;

@Data
public class CaminhaoModel {

    @Expose(serialize = false)
    private int caminhaoId;
    @Expose(serialize = false)
    private boolean caminhaoEstaDisponivel;
    @Expose
    private String caminhaoPlaca;
    @Expose
    private String dataFabricacao;
    @Expose
    private String caminhaoMarca;
    @Expose
    private String caminhaoModelo;
}

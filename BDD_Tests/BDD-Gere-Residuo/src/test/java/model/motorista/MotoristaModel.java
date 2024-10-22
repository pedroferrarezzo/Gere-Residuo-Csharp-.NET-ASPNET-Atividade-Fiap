package model.motorista;

import com.google.gson.annotations.Expose;
import lombok.Data;

@Data
public class MotoristaModel {

    @Expose(serialize = false)
    private int motoristaId;
    @Expose(serialize = false)
    private Boolean motoristaEstaDisponivel;

    @Expose
    private String motoristaNome;

    @Expose
    private String motoristaCpf;

    @Expose
    private String motoristaNrCelular;

    @Expose
    private String motoristaNrCelularDdd;

    @Expose
    private String motoristaNrCelularDdi;
}

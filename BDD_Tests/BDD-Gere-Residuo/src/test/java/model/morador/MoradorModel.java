package model.morador;

import com.google.gson.annotations.Expose;
import lombok.Data;

@Data
public class MoradorModel {

    @Expose(serialize = false)
    private int moradorId;
    @Expose
    private int bairroId;
    @Expose
    private String moradorNome;
    @Expose
    private String moradorEmail;
}

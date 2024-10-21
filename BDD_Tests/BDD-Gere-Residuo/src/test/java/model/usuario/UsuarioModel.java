package model.usuario;
import com.google.gson.annotations.Expose;
import lombok.Data;

@Data
public class UsuarioModel {

    @Expose(serialize = false)
    private int usuarioId;

    @Expose
    private String usuarioNome;

    @Expose
    private String usuarioEmail;

    @Expose(deserialize = false)
    private String usuarioSenha;

    @Expose
    private UsuarioRole usuarioRole;
}

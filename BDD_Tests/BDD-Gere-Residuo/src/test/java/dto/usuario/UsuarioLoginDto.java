package dto.usuario;

import com.google.gson.annotations.Expose;
import model.usuario.UsuarioModel;

public record UsuarioLoginDto(
        @Expose
        String usuarioEmail,

        @Expose
        String usuarioSenha
) {

}

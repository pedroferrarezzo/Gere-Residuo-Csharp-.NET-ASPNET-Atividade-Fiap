package dto.usuario;

import model.usuario.UsuarioModel;

public record UsuarioLoginDto(
        String usuarioEmail,
        String usuarioSenha
) {

}

package hook.usuario;

import io.cucumber.java.AfterAll;
import io.cucumber.java.BeforeAll;
import io.cucumber.java.Scenario;
import services.usuario.CadastroUsuarioService;
import steps.usuario.CadastroUsuarioSteps;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class UsuarioHook {

    private final CadastroUsuarioSteps cadastroUsuarioSteps;

    public UsuarioHook(CadastroUsuarioSteps cadastroUsuarioSteps) {
        this.cadastroUsuarioSteps = cadastroUsuarioSteps;
    }


    @AfterAll
    public void cleanDatabase(Scenario scenario) {
        System.out.println();
    }


}

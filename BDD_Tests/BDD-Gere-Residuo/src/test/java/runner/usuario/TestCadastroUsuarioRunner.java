package runner.usuario;

import io.cucumber.junit.Cucumber;
import io.cucumber.junit.CucumberOptions;
import org.junit.runner.RunWith;

@RunWith(Cucumber.class)
@CucumberOptions(
        features = "src/test/resources/features",
        glue = {"steps.usuario", "hook.usuario"},
        tags = "@USUARIO and @FUNCIONAL and @CREATE",
        plugin = {"html:target/reports/usuario/cadastro-usuario-feature/cucumber-reports.html"}
)
public class TestCadastroUsuarioRunner {

}

package runner.usuario;

import io.cucumber.junit.Cucumber;
import io.cucumber.junit.CucumberOptions;
import org.junit.runner.RunWith;

@RunWith(Cucumber.class)
@CucumberOptions(
        features = "src/test/resources/features",
        glue = {"steps.usuario", "hook.usuario"},
        tags = "@USUARIO and @SMOKE and @AUTH",
        plugin = {"html:target/reports/usuario/autenticacao-usuario-feature/cucumber-reports.html"}
)
public class TestAutenticacaoUsuarioRunner {

}

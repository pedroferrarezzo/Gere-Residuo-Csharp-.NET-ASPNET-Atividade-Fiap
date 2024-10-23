package runner.agenda;

import io.cucumber.junit.Cucumber;
import io.cucumber.junit.CucumberOptions;
import org.junit.runner.RunWith;

@RunWith(Cucumber.class)
@CucumberOptions(
        features = "src/test/resources/features",
        glue = {"steps.usuario", "steps.agenda", "steps.bairro", "steps.morador", "steps.caminhao", "steps.motorista", "hook.agenda", "hook.bairro", "hook.morador", "hook.caminhao", "hook.motorista", "hook.notificacao"},
        tags = "@AGENDA and @REGRESSAO and @CREATE",
        plugin = {"html:target/reports/agenda/cadastro-agenda-feature/cucumber-reports.html"}
)
public class TestCadastroAgendaRunner {

}

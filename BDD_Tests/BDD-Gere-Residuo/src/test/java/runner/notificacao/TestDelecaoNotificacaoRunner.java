package runner.notificacao;

import io.cucumber.junit.Cucumber;
import io.cucumber.junit.CucumberOptions;
import org.junit.runner.RunWith;

@RunWith(Cucumber.class)
@CucumberOptions(
        features = "src/test/resources/features",
        glue = {"steps.usuario", "steps.notificacao", "steps.agenda", "steps.bairro", "steps.morador", "steps.caminhao", "steps.motorista", "hook.agenda", "hook.bairro", "hook.morador", "hook.caminhao", "hook.motorista", "hook.notificacao"},
        tags = "@NOTIFICACAO and @REGRESSAO and @DELETE",
        plugin = {"html:target/reports/notificacao/delecao-agenda-feature/cucumber-reports.html"}
)
public class TestDelecaoNotificacaoRunner {

}

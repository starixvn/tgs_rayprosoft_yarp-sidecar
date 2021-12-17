def message = "";

pipeline {
	agent any
	
	environment {
		BuildConfiguration="Release"
		ImageName="harbor.rpgp.cc/tgs/yarp-sidecar"
	}

	stages {
		stage('Build') {
			steps {
				sh 'dotnet build -c $BuildConfiguration'
			}
		}

		stage('Build Docker Image') {
			steps {
				sh 'dotnet build "YarpSidecar/YarpSidecar.csproj" -c $BuildConfiguration -t:ContainerBuild -p:DockerfileTag="$ImageName:${TAG}" --no-restore '
			}
		}

		stage('Push Docker Image') {
			steps {
				sh 'sudo docker push $ImageName:${TAG}'
			}
		}
	}

	post {
		success {
             sendTelegram("Pipeline: ${currentBuild.fullDisplayName} OK! ${message} \n${currentBuild.getBuildCauses()[0].shortDescription}\n")
        }
        failure {
            sendTelegram("<b>Failed Pipeline:</b> ${currentBuild.fullDisplayName} Something is wrong with ${env.BUILD_URL}/console\n${currentBuild.getBuildCauses()[0].shortDescription}\n")
        }
		always {
			step([$class: 'MSTestPublisher', testResultsFile:"**/*.trx", failOnError: false, keepLongStdio: true])
		}
	}
}

def sendTelegram(message) {
    def encodedMessage = URLEncoder.encode(message, "UTF-8")

    withCredentials([string(credentialsId: "telegramToken", variable: "TOKEN"), string(credentialsId: "telegramChatId", variable: "CHAT_ID")]) {

        response = httpRequest (consoleLogResponseBody: true,
                contentType: 'APPLICATION_JSON',
                httpMode: 'GET',
                url: "https://api.telegram.org/bot${TOKEN}/sendMessage?text=${encodedMessage}&chat_id=${CHAT_ID}",
                validResponseCodes: '200')
        return response
    }
}

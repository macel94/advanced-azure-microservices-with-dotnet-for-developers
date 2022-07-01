# See here for image contents: https://github.com/microsoft/vscode-dev-containers/tree/v0.238.0/containers/dotnet/.devcontainer/base.Dockerfile

# [Choice] .NET version: 6.0, 3.1, 6.0-bullseye, 3.1-bullseye, 6.0-focal, 3.1-focal
ARG VARIANT="6.0-bullseye-slim"
FROM mcr.microsoft.com/vscode/devcontainers/dotnet:0-${VARIANT}

# [Choice] Node.js version: none, lts/*, 18, 16, 14
ARG NODE_VERSION="none"
RUN if [ "${NODE_VERSION}" != "none" ]; then su vscode -c "umask 0002 && . /usr/local/share/nvm/nvm.sh && nvm install ${NODE_VERSION} 2>&1"; fi

# Install powershell
ARG PS_VERSION="7.2.4"
RUN ARCH="$(dpkg --print-architecture)"; \
    if [ "${ARCH}" = "amd64" ]; then \
    PS_BIN="v$PS_VERSION/powershell-$PS_VERSION-linux-x64.tar.gz"; \
    elif [ "${ARCH}" = "arm64" ]; then \
    PS_BIN="v$PS_VERSION/powershell-$PS_VERSION-linux-arm64.tar.gz"; \
    elif [ "${ARCH}" = "armhf" ]; then \
    PS_BIN="v$PS_VERSION/powershell-$PS_VERSION-linux-arm32.tar.gz"; \
    fi; \
    wget https://github.com/PowerShell/PowerShell/releases/download/$PS_BIN -O pwsh.tar.gz; \
    mkdir /usr/local/pwsh && \
    tar Cxvfz /usr/local/pwsh pwsh.tar.gz && \
    rm pwsh.tar.gz

ENV PATH=$PATH:/usr/local/pwsh

RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
    && apt-get install -y --no-install-recommends \
    fish \
    tmux \
    && apt-get clean

ARG USERNAME=vscode

# Download the oh-my-posh binary
RUN mkdir /home/${USERNAME}/bin; \
    wget https://github.com/JanDeDobbeleer/oh-my-posh/releases/latest/download/posh-linux-$(dpkg --print-architecture) -O /home/${USERNAME}/bin/oh-my-posh; \
    chmod +x /home/${USERNAME}/bin/oh-my-posh; \
    chown ${USERNAME}: /home/${USERNAME}/bin;

# Download the fzf binary
RUN wget https://github.com/junegunn/fzf/releases/download/0.29.0/fzf-0.29.0-linux_$(dpkg --print-architecture).tar.gz -O /home/${USERNAME}/bin/fzf; \
    chmod +x /home/${USERNAME}/bin/fzf;

# Setup a neat little PowerShell experience
RUN pwsh -Command Install-Module posh-git -Scope AllUsers -Force; \
    pwsh -Command Install-Module z -Scope AllUsers -Force; \
    pwsh -Command Install-Module PSFzf -Scope AllUsers -Force; \
    pwsh -Command Install-Module Terminal-Icons -Scope AllUsers -Force;

# add the oh-my-posh path to the PATH variable
ENV PATH "$PATH:/home/${USERNAME}/bin"

# Can be used to override the devcontainer prompt default theme:
ENV POSH_THEME="/workspaces/advanced-azure-microservices-with-dotnet/.devcontainer/themes/cloud-native-azure.omp.json"

# Deploy oh-my-posh prompt to Powershell:
COPY Microsoft.PowerShell_profile.ps1 /home/${USERNAME}/.config/powershell/Microsoft.PowerShell_profile.ps1

# Deploy oh-my-posh prompt to Fish:
COPY config.fish /home/${USERNAME}/.config/fish/config.fish

# Everything runs as root during build time, so we want
# to make sure the vscode user can edit these paths too:
RUN chmod 777 -R /home/${USERNAME}/.config

# Override vscode's own Bash prompt with oh-my-posh:
RUN sed -i 's/^__bash_prompt$/#&/' /home/${USERNAME}/.bashrc && \
    echo "eval \"\$(oh-my-posh init bash --config $POSH_THEME)\"" >> /home/${USERNAME}/.bashrc

# Override vscode's own ZSH prompt with oh-my-posh:
RUN echo "eval \"\$(oh-my-posh init zsh --config $POSH_THEME)\"" >> /home/${USERNAME}/.zshrc

RUN apt-get install fontconfig
RUN wget https://github.com/ryanoasis/nerd-fonts/raw/master/patched-fonts/CascadiaCode/Bold/complete/Caskaydia%20Cove%20Bold%20Nerd%20Font%20Complete%20Mono.otf -P /usr/share/fonts
RUN fc-cache -fv

# Set container timezone:
ARG TZ="UTC"
RUN ln -sf /usr/share/zoneinfo/${TZ} /etc/localtime

# [Optional] Uncomment this section to install additional OS packages.
# RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
#     && apt-get -y install --no-install-recommends <your-package-list-here>

# [Optional] Uncomment this line to install global node packages.
# RUN su vscode -c "source /usr/local/share/nvm/nvm.sh && npm install -g <your-package-here>" 2>&1
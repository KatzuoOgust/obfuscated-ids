.PHONY: help build test clean pack publish

PROJECT  := ObfuscatedIds.sln
LIBRARY  := src/ObfuscatedIds/ObfuscatedIds.csproj
ARTIFACTS := artifacts

# NuGet target — override via environment or command line
NUGET_SOURCE ?= https://api.nuget.org/v3/index.json

help: ## Show this help message
	@grep -E '^[a-zA-Z_-]+:.*##' $(MAKEFILE_LIST) \
		| awk 'BEGIN {FS = ":.*##"}; {printf "  \033[36m%-12s\033[0m %s\n", $$1, $$2}'

build: ## Build all projects
	dotnet build $(PROJECT)

test: ## Run all tests
	dotnet test $(PROJECT)

pack: ## Create NuGet package → artifacts/
	dotnet pack $(LIBRARY) --configuration Release
	@echo "Package written to $(ARTIFACTS)/"

publish: pack ## Push NuGet package (requires NUGET_API_KEY)
	@test -n "$(NUGET_API_KEY)" || (echo "Error: NUGET_API_KEY is not set" && exit 1)
	dotnet nuget push $(ARTIFACTS)/*.nupkg \
		--api-key $(NUGET_API_KEY) \
		--source $(NUGET_SOURCE) \
		--skip-duplicate

clean: ## Remove build artifacts and NuGet packages
	dotnet clean $(PROJECT)
	find . -type d \( -name bin -o -name obj \) -exec rm -rf {} +
	rm -rf $(ARTIFACTS)

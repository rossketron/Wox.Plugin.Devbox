all: build install

clean:
	./scripts/make.sh clean

build:
	./scripts/make.sh build

bundle:
	./scripts/make.sh bundle

install:
	./scripts/make.sh install

install-bundle:
	./scripts/make.sh install --from-bundle

uninstall: 
	./scripts/make.sh uninstall

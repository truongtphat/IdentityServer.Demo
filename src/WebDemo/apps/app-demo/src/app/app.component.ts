import { Component } from '@angular/core';
import { AuthConfig, OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'web-demo-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  constructor(private oauthService: OAuthService) {}

  ngOnInit(): void {
    this.oauthService.events.subscribe((event) => {
      if (event.type == 'token_received') {
        if (window.location.pathname.includes('index.html')) {
          window.location.href = 'http://localhost:4200';
        }

        return;
      }
    });

    this.configure();
  }

  public login() {
    this.oauthService.initLoginFlow();
  }

  public logoff() {
    this.oauthService.logOut();
  }

  public get claims() {
    return this.oauthService.getIdentityClaims();
  }

  public get accessToken() {
    return this.oauthService.getAccessToken();
  }

  private configure() {
    const authConfig: AuthConfig = {
      // Login-Url
      issuer: 'https://localhost:5001',

      tokenEndpoint: 'https://localhost:5001/connect/token',

      // URL of the SPA to redirect the user to after login
      redirectUri: window.location.origin + '/index.html',

      // The SPA's id. The SPA is registerd with this id at the auth-server
      // clientId: 'server.code',
      clientId: 'WebApp',

      // Just needed if your auth server demands a secret. In general, this
      // is a sign that the auth server is not configured with SPAs in mind
      // and it might not enforce further best practices vital for security
      // such applications.
      // dummyClientSecret: 'secret',

      responseType: 'code',

      // set the scope for the permissions the client should request
      // The first four are defined by OIDC.
      // Important: Request offline_access to get a refresh token
      // The api scope is a usecase specific one
      scope: 'openid profile ApiDemo',

      showDebugInformation: true,

      postLogoutRedirectUri: 'http://localhost:4200',
    };

    this.oauthService.configure(authConfig);
    this.oauthService.loadDiscoveryDocumentAndTryLogin();
  }
}

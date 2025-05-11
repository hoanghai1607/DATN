import './style.scss';
const Footer = () => {
    return (
        <footer class="footer__footer">
            <div class="footer__logo" title="Twilio">
            </div>
            <ul class="legal__list">
                <li class="legal__list-item">
                    <a href="/legal/tos" class="legal__link">Legal</a></li>
                <li class="legal__list-item">
                    <a href="/legal/privacy" class="legal__link">Privacy</a></li>
                {/* <li class="legal__list-item">
                    <a href="https://www.twilio.org" class="legal__link">Twilio.org</a></li> */}
                <li class="legal__list-item">
                    <a href="/press" class="legal__link">Press &amp; Media</a></li>
                <li class="legal__list-item">
                    <a href="https://signal.twilio.com" class="legal__link">SIGNAL</a></li>
                <li class="legal__list-item">
                    <a href="https://investors.twilio.com/overview/default.aspx" class="legal__link">Investors</a></li>
                <li class="legal__list-item"><a href="/company/jobs" class="legal__link">Jobs</a></li>
            </ul><ul class="footer__copyright legal__list">
                <li class="legal__list-item">

                    Copyright © 2022 Atlassian.
                    <br />All Rights Reserved.
                </li> 
            </ul>
        </footer>
    )
}

export default Footer;